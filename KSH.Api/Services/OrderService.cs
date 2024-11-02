using System.Data;
using System.Linq.Expressions;
using AutoMapper;
using KSH.Api.Constants;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Models.DTO.Response;
using KSH.Api.Repositories;
using KSH.Api.Services.IServices;
using KSH.Api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KSH.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly int pageSize = 20;
        private readonly UserManager<ApplicationUser> _userManager;
        private const int pointRate = 100;
        private readonly ICartService _cartService;
        private readonly IMapboxService _mapboxService;
        public OrderService(IMapper mapper, UnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ICartService cartService, IMapboxService mapboxService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _cartService = cartService;
            _mapboxService = mapboxService;
        }
        #region Service methods
        public async Task<ServiceResponse> GetAsync(OrderStaffGetDTO orderStaffGetDTO)
        {
            try
            {
                var filter = GetFilter(orderStaffGetDTO);
                var sort = GetOrderOrderBy(orderStaffGetDTO);
                var (orders, totalPages) = await _unitOfWork.OrderRepository.GetFilterAsync(filter, sort, skip: pageSize * orderStaffGetDTO.Page, take: pageSize);
                var orderDTOs = _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);

                return new ServiceResponse()
                                .AddDetail("message", "Lấy các order thành công!")
                                .AddDetail("data", new { totalPages, currentPage = orderStaffGetDTO.Page, orders = orderDTOs });
            }
            catch
            {
                return new ServiceResponse()
                                .SetSucceeded(false)
                                .AddDetail("message", "Lấy dữ liệu orders thất bại!bại")
                                .AddError("outOfService", "Không thể lấy dữ liệu order ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetByIdAsync(Guid id, string userId, string role)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
                if (order == null || (userId != order.UserId && role != "staff"))
                {
                    return new ServiceResponse()
                            .SetSucceeded(false)
                            .SetStatusCode(StatusCodes.Status404NotFound)
                            .AddDetail("message", "Lấy thông tin order thất bại!")
                            .AddError("notFound", "Không thể tìm thấy order của bạn, vui lòng kiểm tra lại thông tin!");
                }

                var orderDTO = _mapper.Map<IndividualOrderResponseDTO>(order);

                return new ServiceResponse()
                                .AddDetail("message", "Lấy thông tin order thành công!")
                                .AddDetail("data", new { order = orderDTO });
            }
            catch
            {
                return new ServiceResponse()
                                .SetSucceeded(false)
                                .SetStatusCode(500)
                                .AddDetail("message", "Lấy dữ liệu orders thất bại!")
                                .AddError("outOfService", "Không thể lấy dữ liệu order ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetByCustomerIdAsync(string customerId, OrderGetDTO orderGetDTO)
        {
            try
            {
                var filter = GetByCustomerIdFilter(orderGetDTO, customerId);
                var sort = GetOrderOrderBy(orderGetDTO);
                var (orders, totalPages) = await _unitOfWork.OrderRepository.GetFilterAsync(filter, sort, skip: pageSize * orderGetDTO.Page, take: pageSize);
                var orderDTOs = _mapper.Map<IEnumerable<OrderResponseDTO>>(orders);

                return new ServiceResponse()
                        .AddDetail("message", "Lấy các order thành công!")
                        .AddDetail("data", new { totalPages, currentPage = orderGetDTO.Page, orders = orderDTOs });
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(500)
                        .AddDetail("message", "Lấy dữ liệu orders thất bại!")
                        .AddError("outOfService", "Không thể lấy dữ liệu order ngay lúc này!");
            }
        }
        public async Task<(ServiceResponse, Guid)> CreateByCustomerIdAsync(string userId, bool isUsePoint, string shippingAddress, string phoneNumber, string? note)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userId);
                if (user == null)
                {
                    return (new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status404NotFound)
                        .AddDetail("message", "Tạo đơn hàng thất bại!")
                        .AddError("notFound", "Không tìm thấy tài khoản ngay lúc này!"), Guid.Empty);
                }

                var (carts, totalPages) = await _unitOfWork.CartRepository.GetFilterAsync(
                    u => u.UserId == user.Id,
                    includes: new Func<IQueryable<Cart>, IQueryable<Cart>>[]
                    {
                        c => c.Include(p => p.Package)
                    });

                if (!carts.Any())
                {
                    return (new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status404NotFound)
                        .AddDetail("message", "Tạo đơn hàng thất bại!")
                        .AddError("notFound", "Giỏ hàng của bạn đang trống!"), Guid.Empty);
                }

                var price = carts.Sum(cart => cart.Package.Price * cart.PackageQuantity);
                var point = 0L;
                if (isUsePoint)
                {
                    point = user.Points;
                    if (point > price) { point -= price; }
                    user.Points -= point;
                }
                var distance = await _mapboxService.GetDistanceBetweenAddressAndShop(shippingAddress);
                if (distance == 0)
                {
                    return (new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy khoảng cách thất bại!")
                        .AddError("notFound", "Không thể xác định được địa chỉ cấn lấy khoảng cách!"), Guid.Empty);
                }

                var shippingFee = await _unitOfWork.ShippingFeeRepository.GetShippingFee(distance);
                var totalPrice = price - point + shippingFee.Price;

                var orderId = Guid.NewGuid();
                var orderDTO = new OrderCreateDTO()
                {
                    Id = orderId,
                    UserId = user.Id,
                    CreatedAt = TimeConverter.GetCurrentVietNamTime(),
                    DeliveredAt = null,
                    ShippingStatus = OrderFulfillmentConstants.OrderFailStatus,
                    ShippingAddress = shippingAddress,
                    ShippingFeeId = shippingFee.Id,
                    PhoneNumber = phoneNumber,
                    IsLabDownloaded = false,
                    Price = price,
                    Discount = point,
                    TotalPrice = totalPrice,
                    Note = note,
                    PackageOrders = carts.Select(cart => new PackageOrderCreateDTO
                    {
                        PackageId = cart.PackageId,
                        OrderId = orderId,
                        PackageQuantity = cart.PackageQuantity
                    }).ToList()
                };

                var order = _mapper.Map<UserOrders>(orderDTO);

                await _unitOfWork.OrderRepository.CreateAsync(order);
                await _cartService.RemoveAllAsync(userId);
                await _userManager.UpdateAsync(user);
                return (new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Tạo đơn hàng thành công!"), orderId);

            }
            catch
            {
                return (new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(500)
                        .AddDetail("message", "Tạo đơn hàng thất bại!")
                        .AddError("outOfService", "Không thể tạo đơn hàng ngay lúc này!"), Guid.Empty);
            }
        }
        public async Task<ServiceResponse> UpdateShippingStatus(OrderShippingStatusUpdateDTO getDTO)
        {
            try
            {
                Expression<Func<UserOrders, bool>> orderFilter = (l) => l.Id.Equals(getDTO.Id);
                var (orders, totalOrderPages) = await _unitOfWork.OrderRepository.GetFilterAsync(
                orderFilter,
                null,
                null,
                null,
                query => query.Include(l => l.Payment).Include(l => l.User)
                );
                if (orders == null)
                {
                    return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .AddError("notFound", "Không tìm thấy đơn hàng này!")
                                    .AddDetail("message", "Cập nhật trạng thái giao hàng thất bại");
                }

                var order = orders.FirstOrDefault();
                order!.ShippingStatus = getDTO.ShippingStatus!;
                order!.DeliveredAt = TimeConverter.GetCurrentVietNamTime();

                if (!await _unitOfWork.OrderRepository.UpdateAsync(order))
                {
                    return new ServiceResponse()
                                   .SetSucceeded(false)
                                   .AddError("invalidCredentials", "Thông tin nhập không hợp lệ")
                                   .AddDetail("message", "Cập nhật trạng thái giao hàng thất bại");
                }

                if (order.ShippingStatus != OrderFulfillmentConstants.OrderSuccessStatus)
                {
                    return new ServiceResponse()
                                    .SetSucceeded(true)
                                    .AddDetail("message", "Cập nhật trạng thái giao hàng thành công");
                }
                // cập nhật trạng thái thanh toán của order
                try
                {
                    if (!order.Payment!.Status)
                    {
                        order.Payment.Status = true;
                        if (!await _unitOfWork.PaymentRepository.UpdateAsync(order.Payment))
                        {
                            return new ServiceResponse()
                                            .SetSucceeded(false)
                                            .AddError("outOfService", "Không thể cập nhật trạng thái giao hàng ngay lúc này")
                                            .AddDetail("message", "Cập nhật trạng thái giao hàng thất bại");
                        }
                    }
                }
                catch
                {
                    return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .AddError("outOfService", "Không thể cập nhật ngay lúc này")
                                    .AddError("erorr", "Cập nhật trạng thái giao hàng thất bại")
                                    .AddDetail("message", "Cập nhật trạng thái giao hàng thất bại");
                }

                // Cập nhật điểm cho khách hàng
                try
                {
                    var point = (double)(order.Payment.Amount / 100);
                    order.User.Points += (int)Math.Floor(point);
                    if (!await _unitOfWork.UserRepository.UpdateAsync(order.User))
                    {
                        return new ServiceResponse()
                                        .SetSucceeded(false)
                                        .AddError("outOfService", "Không thể cập nhật trạng thái giao hàng ngay lúc này")
                                        .AddDetail("message", "Cập nhật trạng thái giao hàng thất bại");
                    }
                }
                catch
                {
                    return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .AddError("outOfService", "Không thể cập nhật ngay lúc này")
                                    .AddError("error", "Cập nhật điểm cho khách hàng thất bại")
                                    .AddDetail("message", "Cập nhật trạng thái giao hàng thất bại");
                }

                // Tạo từng order support cho từng LabId có trong những package đã mua trong order 
                try
                {
                    var packageFilter = GetPackageOrderFilter(order.Id);
                    var (packageOrders, packageOrderTotalPages) = await _unitOfWork.PackageOrderRepository.GetFilterAsync(
                        packageFilter,
                        null,
                        null,
                        null
                        );
                    foreach (var packageOrder in packageOrders)
                    {
                        foreach (var lab in packageOrder.Package.PackageLabs)
                        {
                            var orderSupportFilter = GetOrderSupportFilter(order.Id, lab.LabId, lab.PackageId);
                            var (checkOrderSupportExited, totalOrderSupportPages) = await _unitOfWork.OrderSupportRepository.GetFilterAsync(
                                orderSupportFilter,
                                null,
                                null,
                                null
                                );
                            if (checkOrderSupportExited.Count() == 0)
                            {
                                _unitOfWork.OrderSupportRepository.Create(new OrderSupport() { Id = Guid.NewGuid(), LabId = lab.LabId, OrderId = order.Id, PackageId = lab.PackageId, RemainSupportTimes = lab.Lab.MaxSupportTimes * packageOrder.PackageQuantity });
                            }
                        }
                    }
                }
                catch
                {
                    return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .AddError("outOfService", "Không thể cập nhật ngay lúc này")
                                    .AddError("error", "Tạo OrderSupport thất bại")
                                    .AddDetail("message", "Cập nhật trạng thái giao hàng thất bại");
                }

                return new ServiceResponse()
                                .SetSucceeded(true)
                                .AddDetail("message", "Cập nhật trạng thái giao hàng thành công");
            }
            catch
            {
                return new ServiceResponse()
                                .SetSucceeded(false)
                                .AddError("outOfService", "Không thể cập nhật ngay lúc này")
                                .AddDetail("message", "Cập nhật trạng thái giao hàng thất bại");
            }
        }
        public async Task<ServiceResponse> CancelShippingStatus(OrderShippingStatusUpdateDTO getDTO)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(getDTO.Id);
                if (order == null)
                {
                    return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .SetStatusCode(StatusCodes.Status404NotFound)
                                    .AddError("notFound", "Không tìm thấy đơn hàng này!")
                                    .AddDetail("message", "Cập nhật trạng thái giao hàng thất bại");
                }
                if (order.ShippingStatus.Equals(OrderFulfillmentConstants.OrderDeliveringStatus))
                {
                    return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .SetStatusCode(StatusCodes.Status451UnavailableForLegalReasons)
                                    .AddError("Unavailable", "Không thể sử dụng ngày lúc này!")
                                    .AddDetail("message", "Không thể hủy đơn hàng lúc này");
                }
                order.ShippingStatus = OrderFulfillmentConstants.OrderFailStatus;
                if (!await _unitOfWork.OrderRepository.UpdateAsync(order))
                {
                    return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .SetStatusCode(StatusCodes.Status500InternalServerError)
                                    .AddError("outOfService", "Không thể cập nhật ngay lúc này")
                                    .AddError("error", "hủy đơn hàng thất bại thất bại")
                                    .AddDetail("message", "Khách hàng hủy đơn hàng thất bại thất bại");
                }
                return new ServiceResponse()
                                    .SetSucceeded(true)
                                    .AddDetail("message", "Hủy đơn hàng thành công");
            }
            catch
            {
                return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .SetStatusCode(StatusCodes.Status500InternalServerError)
                                    .AddError("outOfService", "Không thể cập nhật ngay lúc này")
                                    .AddError("error", "hủy đơn hàng thất bại thất bại")
                                    .AddDetail("message", "Khách hàng hủy đơn hàng thất bại thất bại");
            }
        }
        public async Task<ServiceResponse> GetShippingFee(string address)
        {
            try
            {
                var distance = await _mapboxService.GetDistanceBetweenAddressAndShop(address);
                if (distance == 0)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy khoảng cách thất bại!")
                        .AddError("notFound", "Không thể xác định được địa chỉ cấn lấy khoảng cách!");
                }

                var shippingFee = await _unitOfWork.ShippingFeeRepository.GetShippingFee(distance);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy phí giao hàng thành công!")
                    .AddDetail("data", new { shippingFee.Price });

            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status500InternalServerError)
                        .AddDetail("message", "Lâý phí giao hàng thất bại!")
                        .AddError("outOfService", "Không thể lấy phí giao hàng ngay lúc này!");
            }
        }

        #endregion

        #region Methods that help service
        private Expression<Func<UserOrders, bool>>? GetFilter(OrderStaffGetDTO orderStaffGetDTO)
        {
            orderStaffGetDTO.CreatedFrom = TimeConverter.ToVietNamTime(orderStaffGetDTO.CreatedFrom);
            if (orderStaffGetDTO.CreatedTo < DateTimeOffset.MaxValue)
            {
                orderStaffGetDTO.CreatedTo = TimeConverter.ToVietNamTime(orderStaffGetDTO.CreatedTo.AddDays(1).AddTicks(-1));
            }
            return o => o.CreatedAt >= orderStaffGetDTO.CreatedFrom &&
                        o.CreatedAt <= orderStaffGetDTO.CreatedTo &&
                        o.User.Email!.Contains(orderStaffGetDTO.CustomerEmail ?? "") &&
                        o.TotalPrice >= orderStaffGetDTO.FromAmount &&
                        o.TotalPrice <= orderStaffGetDTO.ToAmount &&
                        o.ShippingStatus.Contains(orderStaffGetDTO.ShippingStatus ?? "");
        }

        private Expression<Func<UserOrders, bool>>? GetByCustomerIdFilter(OrderGetDTO orderGetDTO, string customerId)
        {
            orderGetDTO.CreatedFrom = TimeConverter.ToVietNamTime(orderGetDTO.CreatedFrom);
            if (orderGetDTO.CreatedTo < DateTimeOffset.MaxValue)
            {
                orderGetDTO.CreatedTo = TimeConverter.ToVietNamTime(orderGetDTO.CreatedTo.AddDays(1).AddTicks(-1));
            }
            return o => o.CreatedAt >= orderGetDTO.CreatedFrom &&
                        o.CreatedAt <= orderGetDTO.CreatedTo &&
                        o.UserId == customerId &&
                        o.TotalPrice >= orderGetDTO.FromAmount &&
                        o.TotalPrice <= orderGetDTO.ToAmount &&
                        o.ShippingStatus.Contains(orderGetDTO.ShippingStatus ?? "");
        }
        private Expression<Func<PackageOrder, bool>> GetPackageOrderFilter(Guid orderId)
        {
            return (l) => l.OrderId.Equals(orderId);
            ;
        }
        private Expression<Func<OrderSupport, bool>> GetOrderSupportFilter(Guid orderId, Guid labId, int packageId)
        {
            return (l) => l.OrderId.Equals(orderId) &&
                          l.LabId.Equals(labId) &&
                          l.PackageId.Equals(packageId);
        }

        private Func<IQueryable<UserOrders>, IOrderedQueryable<UserOrders>> GetOrderOrderBy(OrderGetDTO orderGetDTO)
        {
            return query =>
            {
                if (orderGetDTO.SortFields == null || orderGetDTO.SortOrders == null)
                {
                    return query.OrderByDescending(o => o.CreatedAt);
                }

                var fields = orderGetDTO.SortFields.Split(",");
                var fieldOrders = orderGetDTO.SortOrders.Split(",");
                if (fields.Length == 0 || fieldOrders.Length == 0 || fields.Length != fieldOrders.Length)
                {
                    return query.OrderByDescending(o => o.CreatedAt);
                }

                IOrderedQueryable<UserOrders>? orderByQuery = null;
                for (int i = 0; i < fields.Length; i++)
                {
                    var field = fields[i].ToLower();
                    var fieldOrder = fieldOrders[i].ToLower();
                    orderByQuery = orderByQuery == null ? ApplyOrderBy(query, field, fieldOrder) : ApplyThenOrderBy(orderByQuery, field, fieldOrder);
                }

                return orderByQuery ?? query.OrderByDescending(o => o.CreatedAt);
            };
        }

        private IOrderedQueryable<UserOrders>? ApplyOrderBy(IQueryable<UserOrders> query, string field, string fieldOrder)
        {
            return (field, fieldOrder) switch
            {
                ("createdat", "asc") => query.OrderBy(o => o.CreatedAt),
                ("createdat", "desc") => query.OrderByDescending(o => o.CreatedAt),
                _ => null,
            };
        }

        private IOrderedQueryable<UserOrders>? ApplyThenOrderBy(IOrderedQueryable<UserOrders> orderByQuery, string field, string fieldOrder)
        {
            return (field, fieldOrder) switch
            {
                ("createdat", "asc") => orderByQuery.ThenBy(o => o.CreatedAt),
                ("createdat", "desc") => orderByQuery.ThenByDescending(o => o.CreatedAt),
                _ => orderByQuery,
            };
        }
        #endregion
    }
}