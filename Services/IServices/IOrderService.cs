using kit_stem_api.Models.DTO.Request;

namespace kit_stem_api.Services.IServices
{
    public interface IOrderService
    {
        Task<ServiceResponse> GetAsync(OrderStaffGetDTO orderStaffGetDTO);
        Task<ServiceResponse> GetByCustomerIdAsync(string customerId, OrderGetDTO orderGetDTO);
        Task<ServiceResponse> GetByIdAsync(Guid id, string userId, string role);
        Task<ServiceResponse> CreateByCustomerIdAsync(string userId, bool isUsePoint, string note);
    }
}