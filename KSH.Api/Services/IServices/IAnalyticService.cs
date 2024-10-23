namespace KSH.Api.Services.IServices
{
    public interface IAnalyticService
    {
        Task<ServiceResponse> GetOrderData(DateTimeOffset fromDate, DateTimeOffset toDate, string? shippingStatus);
        Task<ServiceResponse> GetTopKitSale(DateTimeOffset fromDate, DateTimeOffset toDate, string? shippinStatus);
    }
}