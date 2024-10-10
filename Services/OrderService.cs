using System.Drawing;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.Wasm;
using AutoMapper;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Models.DTO.Response;
using kit_stem_api.Repositories;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly int pageSize = 20;
        private readonly UserManager<ApplicationUser> _userManager;
        private const int pointRate = 100;
        public OrderService(IMapper mapper, UnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        #region Service methods
        public async Task<ServiceResponse> GetAsync(OrderStaffGetDTO orderStaffGetDTO)
        {
            try
            {
                var filter = GetFilter(orderStaffGetDTO);
                var (orders, totalPages) = await _unitOfWork.OrderRepository.GetFilterAsync(filter, null, skip: pageSize * orderStaffGetDTO.Page, take: pageSize);
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

                var orderDTO = _mapper.Map<OrderResponseDTO>(order);

                return new ServiceResponse()
                        .AddDetail("message", "Lấy thông tin order thành công!")
                        .AddDetail("data", new { order = orderDTO });
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(500)
                        .AddDetail("message", "Lấy dữ liệu orders thất bại!bại")
                        .AddError("outOfService", "Không thể lấy dữ liệu order ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetByCustomerIdAsync(string customerId, OrderGetDTO orderGetDTO)
        {
            try
            {
                var filter = GetByCustomerIdFilter(orderGetDTO, customerId);
                var (orders, totalPages) = await _unitOfWork.OrderRepository.GetFilterAsync(filter, null, skip: pageSize * orderGetDTO.Page, take: pageSize);
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
        public async Task<(ServiceResponse, Guid)> CreateByCustomerIdAsync(string userId, bool isUsePoint, string note)
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
                int price = carts.Sum(cart => cart.Package.Price * cart.PackageQuantity);

                int point = 0;
                if (isUsePoint) 
                { 
                    point = user.Points;
                    user.Points -= point;
                }

                int totalPrice = price - point;

                var orderId = Guid.NewGuid();
                var orderDTO = new OrderCreateDTO()
                {
                    Id = orderId,
                    UserId = user.Id,
                    CreatedAt = DateTimeOffset.Now,
                    DeliveredAt = null,
                    ShippingStatus = "fail",
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
                user.Points += totalPrice / pointRate;

                await _unitOfWork.OrderRepository.CreateAsync(order);
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
        #endregion

        #region Methods that help service
        private Expression<Func<UserOrders, bool>>? GetFilter(OrderStaffGetDTO orderStaffGetDTO)
        {
            return o => o.CreatedAt >= orderStaffGetDTO.CreatedFrom &&
                        o.CreatedAt <= orderStaffGetDTO.CreatedTo &&
                        o.User.Email!.Contains(orderStaffGetDTO.CustomerEmail ?? "") &&
                        o.TotalPrice >= orderStaffGetDTO.FromAmount &&
                        o.TotalPrice <= orderStaffGetDTO.ToAmount;
        }

        private Expression<Func<UserOrders, bool>>? GetByCustomerIdFilter(OrderGetDTO orderGetDTO, string customerId)
        {
            return o => o.CreatedAt >= orderGetDTO.CreatedFrom &&
                        o.CreatedAt <= orderGetDTO.CreatedTo &&
                        o.UserId == customerId &&
                        o.TotalPrice >= orderGetDTO.FromAmount &&
                        o.TotalPrice <= orderGetDTO.ToAmount;
        }
        #endregion
    }
}