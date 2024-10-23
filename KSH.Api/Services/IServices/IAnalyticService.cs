using KSH.Api.Models.DTO.Request;

namespace KSH.Api.Services.IServices
{
    public interface IAnalyticService
    {
        Task<ServiceResponse> GetOrderData(DateTimeOffset fromDate, DateTimeOffset toDate, string? shippingStatus);
        Task<ServiceResponse> GetTopPackageSale(TopPackageSaleGetDTO packageSaleGetDTO);
    }
}