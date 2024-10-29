using KSH.Api.Models.DTO.Request;
using KSH.Api.Models.DTO.Response;
using KSH.Api.Repositories;
using KSH.Api.Services.IServices;
using KSH.Api.Utils;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<ServiceResponse> GetProfit(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            try
            {
                DateTimeOffset currentTime = TimeConverter.GetCurrentVietNamTime();
                if (toDate <= fromDate || fromDate > currentTime || toDate > currentTime)
                {
                    return new ServiceResponse()
                    .SetSucceeded(false)
                    .SetStatusCode(StatusCodes.Status400BadRequest)
                    .AddDetail("message", "Lấy dữ liệu lợi nhuận thất bại!")
                    .AddError("invalidCredentials", "Ngày bắt đầu và ngày kết thúc không hợp lệ!");
                }

                var purchaseCost = await GetPurchaseCostHelper(fromDate, toDate);
                var revenue = await _unitOfWork.OrderRepository.SumTotalOrder(fromDate, toDate);
                var profit = revenue - purchaseCost;

                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy dữ liệu lợi nhuận thành công!")
                    .AddDetail("data", new { profit });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .SetStatusCode(StatusCodes.Status500InternalServerError)
                    .AddDetail("message", "Lấy dữ liệu lợi nhuận thất bại!")
                    .AddError("outOfService", "Không thể lấy dữ liệu lợi nhuận ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetRevenue(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            try
            {
                DateTimeOffset currentTime = TimeConverter.GetCurrentVietNamTime();
                if (toDate <= fromDate || fromDate > currentTime || toDate > currentTime)
                {
                    return new ServiceResponse()
                    .SetSucceeded(false)
                    .SetStatusCode(StatusCodes.Status400BadRequest)
                    .AddDetail("message", "Lấy dữ liệu doanh thu thất bại!")
                    .AddError("invalidCredentials", "Ngày bắt đầu và ngày kết thúc không hợp lệ!");
                }

                var revenue = await _unitOfWork.OrderRepository.SumTotalOrder(fromDate, toDate);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy dữ liệu doanh thu thành công!")
                    .AddDetail("data", revenue);
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .SetStatusCode(StatusCodes.Status500InternalServerError)
                    .AddDetail("message", "Lấy dữ liệu doanh thu thất bại!")
                    .AddError("outOfService", "Không thể lấy dữ liệu doanh thu ngay lúc này!");
            }
        }
        public async Task<ServiceResponse> GetTopPackageSale(TopPackageSaleGetDTO packageSaleGetDTO)
        {
            try
            {
                var (packages, labsSales) = await _unitOfWork.PackageOrderRepository.GetTopPackageSale(packageSaleGetDTO);
                for (int i = 0; i < labsSales.Count(); i++)
                {
                    for (int j = 0; j < packages.Count(); j++)
                    {
                        if(packages.ElementAt(j).PackageId == labsSales.ElementAt(i).PackageId)
                        {
                            packages.ElementAt(j).TotalProfit += labsSales.ElementAt(i).TotalLabPrice;
                        }
                    }
                }
                return new ServiceResponse()
                                .SetSucceeded(true)
                                .AddDetail("data", new { packages = packages });
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

        #region Helper
        private async Task<long> GetPurchaseCostHelper(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            var listOrderId = await _unitOfWork.OrderRepository.GetOrderId(fromDate, toDate);
            var listPackageOrder = await _unitOfWork.PackageOrderRepository.GetPackageOrder(listOrderId);
            List<PackageDTO> listKitInPackage = new List<PackageDTO>();

            foreach (var packageOrder in listPackageOrder)
            {
                var kitId = await _unitOfWork.PackageRepository.GetByPackageId(packageOrder.PackageId);
                PackageDTO packageDTO = new PackageDTO()
                {
                    KitId = kitId,
                    Quantity = packageOrder.PackageQuantity,
                };
                listKitInPackage!.Add(packageDTO);
            }

            List<KitDTO> listKit = new List<KitDTO>();

            foreach (var kit in listKitInPackage)
            {
                var purchaseCost = await _unitOfWork.KitRepository.GetPurchaseCostById(kit.KitId);
                KitDTO kitDTO = new KitDTO()
                {
                    Id = kit.KitId,
                    PurchaseCost = purchaseCost,
                    Quantity = kit.Quantity,
                };
                listKit.Add(kitDTO);
            }

            var sumOfKit = listKit.Sum(k => k.PurchaseCost * k.Quantity);

            var listLabInPackage = new List<PackageLabDTO>();
            foreach (var packageLab in listPackageOrder)
            {
                var labIds = await _unitOfWork.PackageLabRepository.GetByPackageId(packageLab.PackageId);
                foreach (var labId in labIds)
                {
                    PackageLabDTO packageLabDTO = new PackageLabDTO()
                    {
                        Id = labId,
                        Quantity = packageLab.PackageQuantity,
                    };
                    listLabInPackage!.Add(packageLabDTO);
                }
            }

            var listLab = new List<LabDTO>();

            foreach (var lab in listLabInPackage)
            {
                var purchaseCost = await _unitOfWork.LabRepository.GetPurchaseCostById(lab.Id);
                LabDTO labDTO = new LabDTO()
                {
                    Id = lab.Id,
                    PurchaseCost = purchaseCost,
                    Quantity = lab.Quantity,
                };
                listLab.Add(labDTO);
            }

            var sumOfLab = listLab.Sum(l => l.PurchaseCost * l.Quantity);

            var total = sumOfKit + sumOfLab;
            return total;
        }
        #endregion
    }
}