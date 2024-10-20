using KSH.Api.Models.DTO.Request;

namespace KSH.Api.Services.IServices
{
    public interface IAnalyticService
    {
        Task<ServiceResponse> GetOrderData(AnalyticOrderDTO analyticOrderDTO);
    }
}