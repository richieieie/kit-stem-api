using System.Linq.Expressions;
using AutoMapper;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Models.DTO.Response;
using kit_stem_api.Repositories;
using kit_stem_api.Services.IServices;

namespace kit_stem_api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        private readonly int pageSize = 20;
        public OrderService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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