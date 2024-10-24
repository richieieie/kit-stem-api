using KSH.Api.Models.DTO.Request;

namespace KSH.Api.Services.IServices
{
    public interface IOrderService
    {
        Task<ServiceResponse> GetAsync(OrderStaffGetDTO orderStaffGetDTO);
        Task<ServiceResponse> GetByCustomerIdAsync(string customerId, OrderGetDTO orderGetDTO);
        Task<ServiceResponse> GetByIdAsync(Guid id, string userId, string role);
        Task<(ServiceResponse, Guid)> CreateByCustomerIdAsync(string userId, bool isUsePoint, string shippingAddress, string phoneNumber, string note);
        Task<ServiceResponse> UpdateShippingStatus(OrderShippingStatusUpdateDTO getDTO);
        Task<ServiceResponse> GetShippingFee(string address);
    }
}