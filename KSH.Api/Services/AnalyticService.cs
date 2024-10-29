using KSH.Api.Constants;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Models.DTO.Response;
using KSH.Api.Repositories;
using KSH.Api.Services.IServices;
using KSH.Api.Utils;
using System.Globalization;

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
                if (toDate < DateTimeOffset.MaxValue)
                {
                    toDate.AddDays(1).AddTicks(-1);
                }
                var shippingStatus = analyticOrderDTO.ShippingStatus;
                var numberOfOrders = await _unitOfWork.OrderRepository.CountTotalOrders(fromDate, toDate, shippingStatus);
                return serviceResponse
                        .AddDetail("message", "Lấy dữ liệu đơn hàng thành công!công")
                        .AddDetail("data", new { numberOfOrders });
            }
            catch
            {
                return serviceResponse
                        .AddDetail("message", "Lấy dữ liệu đơn hàng thất bại!")
                        .AddError("outOfService", "Không lấy được dữ liệu đơn hàng ngay lúc này");
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
                        .AddDetail("message", "Lâý danh sách các gói kit bán chạy nhất thất bại!")
                        .AddError("outOfService", "Không lấy được các gói kit bán chạy nhất ở thời điểm hiện tại");
            }
        }

        public async Task<ServiceResponse> GetProfit(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            try
            {
                // DateTimeOffset currentTime = TimeConverter.GetCurrentVietNamTime();
                // if (toDate <= fromDate || fromDate > currentTime || toDate > currentTime)
                // {
                //     return new ServiceResponse()
                //     .SetSucceeded(false)
                //     .SetStatusCode(StatusCodes.Status400BadRequest)
                //     .AddDetail("message", "Lấy dữ liệu lợi nhuận thất bại!")
                //     .AddError("invalidCredentials", "Ngày bắt đầu và ngày kết thúc không hợp lệ!");
                // }

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
                // DateTimeOffset currentTime = TimeConverter.GetCurrentVietNamTime();
                // if (toDate <= fromDate || fromDate > currentTime || toDate > currentTime)
                // {
                //     return new ServiceResponse()
                //     .SetSucceeded(false)
                //     .SetStatusCode(StatusCodes.Status400BadRequest)
                //     .AddDetail("message", "Lấy dữ liệu doanh thu thất bại!")
                //     .AddError("invalidCredentials", "Ngày bắt đầu và ngày kết thúc không hợp lệ!");
                // }

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
                var packages = await _unitOfWork.PackageOrderRepository.GetTopPackageSale(packageSaleGetDTO);
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

        public async Task<ServiceResponse> GetRevenuePerYear(int year)
        {
            try
            {
                var monthDTO = GetDateHelper(year);

                var yearDTO = new YearDTO()
                {
                    January = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.JanuaryStart, monthDTO.JanuaryEnd),
                    February = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.FebruaryStart, monthDTO.FebruaryEnd),
                    March = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.MarchStart, monthDTO.MarchEnd),
                    April = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.AprilStart, monthDTO.AprilEnd),
                    May = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.MayStart, monthDTO.MayEnd),
                    June = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.JuneStart, monthDTO.JuneEnd),
                    July = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.JulyStart, monthDTO.JulyEnd),
                    August = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.AugustStart, monthDTO.AugustEnd),
                    September = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.SeptemberStart, monthDTO.SeptemberEnd),
                    October = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.OctoberStart, monthDTO.OctoberEnd),
                    November = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.NovemberStart, monthDTO.NovemberEnd),
                    December = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.DecemberStart, monthDTO.DecemberEnd)
                };

                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy doanh thu theo năm thành công!")
                    .AddDetail("data", new { yearDTO });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .SetStatusCode(StatusCodes.Status500InternalServerError)
                    .AddDetail("message", "Lấy doanh thu theo năm thất bại!")
                    .AddError("outOfService", "Không thể lấy doanh thu theo năm lúc này!");
            }

        }

        public async Task<ServiceResponse> GetProfitPerYear(int year)
        {
            try
            {
                var monthDTO = GetDateHelper(year);

                var yearDTO = new YearDTO()
                {
                    January = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.JanuaryStart, monthDTO.JanuaryEnd)
                    - await GetPurchaseCostHelper(monthDTO.JanuaryStart, monthDTO.JanuaryEnd),
                    February = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.FebruaryStart, monthDTO.FebruaryEnd)
                    - await GetPurchaseCostHelper(monthDTO.FebruaryStart, monthDTO.FebruaryEnd),
                    March = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.MarchStart, monthDTO.MarchEnd)
                    - await GetPurchaseCostHelper(monthDTO.MarchStart, monthDTO.MarchEnd),
                    April = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.AprilStart, monthDTO.AprilEnd)
                    - await GetPurchaseCostHelper(monthDTO.AprilStart, monthDTO.AprilEnd),
                    May = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.MayStart, monthDTO.MayEnd)
                    - await GetPurchaseCostHelper(monthDTO.MayStart, monthDTO.MayEnd),
                    June = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.JuneStart, monthDTO.JuneEnd)
                    - await GetPurchaseCostHelper(monthDTO.JuneStart, monthDTO.JuneEnd),
                    July = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.JulyStart, monthDTO.JulyEnd)
                    - await GetPurchaseCostHelper(monthDTO.JulyStart, monthDTO.JulyEnd),
                    August = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.AugustStart, monthDTO.AugustEnd)
                    - await GetPurchaseCostHelper(monthDTO.AugustStart, monthDTO.AugustEnd),
                    September = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.SeptemberStart, monthDTO.SeptemberEnd)
                    - await GetPurchaseCostHelper(monthDTO.SeptemberStart, monthDTO.SeptemberEnd),
                    October = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.OctoberStart, monthDTO.OctoberEnd)
                    - await GetPurchaseCostHelper(monthDTO.OctoberStart, monthDTO.OctoberEnd),
                    November = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.NovemberStart, monthDTO.NovemberEnd)
                    - await GetPurchaseCostHelper(monthDTO.NovemberStart, monthDTO.NovemberEnd),
                    December = await _unitOfWork.OrderRepository.SumTotalOrder(monthDTO.DecemberStart, monthDTO.DecemberEnd)
                    - await GetPurchaseCostHelper(monthDTO.DecemberStart, monthDTO.DecemberEnd)
                };

                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy lợi nhuận theo năm thành công!")
                    .AddDetail("data", new { yearDTO });
            }
            catch
            {
                return new ServiceResponse()
                   .SetSucceeded(false)
                   .SetStatusCode(StatusCodes.Status500InternalServerError)
                   .AddDetail("message", "Lấy lợi nhuận theo năm thất bại!")
                   .AddError("outOfService", "Không thể lấy lợi nhuận theo năm lúc này!");
            }
        }


        #region Helper
        private async Task<long> GetPurchaseCostHelper(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            if (toDate < DateTimeOffset.MaxValue)
            {
                toDate.AddDays(1).AddTicks(-1);
            }
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

        private MonthDTO GetDateHelper(int year)
        {
            DateTimeOffset februaryEnd;
            if (DateTime.IsLeapYear(year))
            {
                februaryEnd = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.FebruaryLeapEnd}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN")));
            }
            else
            {
                februaryEnd = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.FebruaryEnd}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN")));
            }

            MonthDTO monthDTO = new MonthDTO()
            {
                JanuaryStart = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.JanuaryStart}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),
                JanuaryEnd = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.JanuaryEnd}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),

                FebruaryStart = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.FebruaryStart}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),
                FebruaryEnd = februaryEnd,

                MarchStart = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.MarchStart}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),
                MarchEnd = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.MarchEnd}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),

                AprilStart = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.AprilStart}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),
                AprilEnd = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.AprilEnd}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),

                MayStart = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.MayStart}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),
                MayEnd = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.MayEnd}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),

                JuneStart = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.JuneStart}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),
                JuneEnd = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.JuneEnd}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),

                JulyStart = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.JulyStart}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),
                JulyEnd = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.JulyEnd}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),

                AugustStart = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.AugustStart}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),
                AugustEnd = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.AugustEnd}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),

                SeptemberStart = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.SeptemberStart}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),
                SeptemberEnd = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.SeptemberEnd}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),

                OctoberStart = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.OctoberStart}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),
                OctoberEnd = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.OctoberEnd}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),

                NovemberStart = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.NovemberStart}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),
                NovemberEnd = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.NovemberEnd}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),

                DecemberStart = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.DecemberStart}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN"))),
                DecemberEnd = new DateTimeOffset(DateTime.ParseExact($"{year}-{DateConstants.DecemberEnd}", "yyyy-dd-MM HH:mm:ss", new CultureInfo("vi-VN")))
            };

            return monthDTO;
        }


        #endregion
    }
}