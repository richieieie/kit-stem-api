using KSH.Api.Models.DTO.Request;

namespace KSH.Api.Services.IServices
{
    public interface IAnalyticService
    {
        Task<ServiceResponse> GetOrderData(AnalyticOrderDTO analyticOrderDTO);
        Task<ServiceResponse> GetTopPackageByYear(int top, int year);
    }
}