using KSH.Api.Models.DTO.Request;
using System;
using System.Threading.Tasks;

namespace KSH.Api.Services.IServices
{
    public interface IAnalyticService
    {
        Task<ServiceResponse> GetOrderData(AnalyticOrderDTO analyticOrderDTO);
        Task<ServiceResponse> GetTopPackageByYear(int top, int year);
        Task<ServiceResponse> GetRevenue(DateTimeOffset fromDate, DateTimeOffset toDate);
        Task<ServiceResponse> GetProfit(DateTimeOffset fromDate, DateTimeOffset toDate);
        Task<ServiceResponse> GetTopPackageSale(TopPackageSaleGetDTO packageSaleGetDTO);
    }
}