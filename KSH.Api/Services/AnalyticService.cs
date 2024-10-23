using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Models.DTO.Response;
using KSH.Api.Repositories;
using KSH.Api.Services.IServices;
using KSH.Api.Utils;

namespace KSH.Api.Services
{
    public class AnalyticService : IAnalyticService
    {
        private readonly UnitOfWork _unitOfWork;
        public AnalyticService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResponse> GetOrderData(AnalyticOrderDTO analyticOrderDTO)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var fromDate = TimeConverter.ToVietNamTime(analyticOrderDTO.FromDate);
                var toDate = TimeConverter.ToVietNamTime(analyticOrderDTO.ToDate);
                var shippingStatus = analyticOrderDTO.ShippingStatus;
                var numberOfOrders = await _unitOfWork.OrderRepository.CountTotalOrders(fromDate, toDate, shippingStatus);
                return serviceResponse
                        .AddDetail("message", "")
                        .AddDetail("data", new { numberOfOrders });
            }
            catch
            {
                return serviceResponse
                        .AddDetail("message", "")
                        .AddError("outOfService", "");
            }
        }

        public async Task<ServiceResponse> GetTopPackageByYear(int top, int year)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var topPackages = await _unitOfWork.PackageOrderRepository.GetTopPackageOrderedByYear(top, year);
                var topPackageDTOs = topPackages.Select(ps =>
                {
                    dynamic item = ps;
                    return new PackageSalesResponseDTO()
                    {
                        PackageId = item.PackageId,
                        PackageName = item.PackageName,
                        KitId = item.KitId,
                        KitName = item.KitName,
                        SoldQuantity = item.SoldQuantity
                    };
                }).ToList();

                return serviceResponse
                        .AddDetail("message", "Lâý danh sách các gói kit bán chạy nhất thành công!")
                        .AddDetail("data", new { topPackages = topPackageDTOs });
            }
            catch
            {
                return serviceResponse
                        .AddDetail("message", "")
                        .AddError("outOfService", "");
            }
        }
    }
}