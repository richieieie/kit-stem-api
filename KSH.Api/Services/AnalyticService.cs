using KSH.Api.Repositories;
using KSH.Api.Services.IServices;

namespace KSH.Api.Services
{
    public class AnalyticService : IAnalyticService
    {
        private readonly UnitOfWork _unitOfWork;
        public AnalyticService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<ServiceResponse> GetOrderData(DateTimeOffset fromDate, DateTimeOffset toDate, string? shippingStatus)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse> GetTopKitSale(DateTimeOffset fromDate, DateTimeOffset toDate, string? shippinStatus)
        {
            throw new NotImplementedException();
        }
    }
}