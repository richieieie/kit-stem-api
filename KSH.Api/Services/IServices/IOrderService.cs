using KST.Api.Models.DTO.Request;

namespace KST.Api.Services.IServices
{
    public interface IOrderService
    {
        Task<ServiceResponse> GetAsync(OrderStaffGetDTO orderStaffGetDTO);
        Task<ServiceResponse> GetByCustomerIdAsync(string customerId, OrderGetDTO orderGetDTO);
        Task<ServiceResponse> GetByIdAsync(Guid id, string userId, string role);
        Task<(ServiceResponse, Guid)> CreateByCustomerIdAsync(string userId, bool isUsePoint, string note);
        Task<ServiceResponse> UpdateShippingStatus(OrderUpdateShippingStatusDTO getDTO);
    }
}