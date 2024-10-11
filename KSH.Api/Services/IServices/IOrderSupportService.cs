using KST.Api.Models.DTO.Request;

namespace KST.Api.Services.IServices
{
    public interface IOrderSupportService
    {
        Task<ServiceResponse> GetAsync(OrderSupportGetDTO getDTO);
    }
}
