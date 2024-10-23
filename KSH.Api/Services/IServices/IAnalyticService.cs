namespace KSH.Api.Services.IServices
{
    public interface IAnalyticService
    {
        Task<ServiceResponse> GetOrderData(DateTimeOffset fromDate, DateTimeOffset toDate, string? shippingStatus);
        Task<ServiceResponse> GetRevenue(DateTimeOffset fromDate, DateTimeOffset toDate);
        Task<ServiceResponse> GetProfit(DateTimeOffset fromDate, DateTimeOffset toDate);

    }
}