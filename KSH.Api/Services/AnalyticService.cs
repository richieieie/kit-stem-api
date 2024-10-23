using KSH.Api.Models.DTO.Request;
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

        public async Task<ServiceResponse> GetTopPackageSale(TopPackageSaleGetDTO packageSaleGetDTO)
        {
            try
            {
                var packages = await _unitOfWork.PackageOrderRepository.GetTopPackageSale(packageSaleGetDTO);
                return new ServiceResponse()
                                .SetSucceeded(true)
                                .AddDetail("data", new {packages = packages});
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .SetStatusCode(StatusCodes.Status500InternalServerError)
                    .AddError("outOfService", "Không thể lấy danh sách package lúc này")
                    .AddDetail("message", "Lấy danh sách top thất bại");
            }
        }
    }
}