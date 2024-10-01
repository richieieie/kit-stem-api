using kit_stem_api.Models.DTO.Request;

namespace kit_stem_api.Services.IServices
{
    public interface IOrderService
    {
        Task<ServiceResponse> GetAsync(OrderGetDTO orderGetDTO);
        Task<ServiceResponse> GetByUserIdAsync();
        Task<ServiceResponse> GetByIdAsync();
    }
}