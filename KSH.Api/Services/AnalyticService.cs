using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Repositories;
using KSH.Api.Services.IServices;
using KSH.Api.Utils;
using Org.BouncyCastle.Bcpg;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.ConstrainedExecution;

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

        public async Task<ServiceResponse> GetPurchaseCost(DateTimeOffset fromDate, DateTimeOffset toDate)
        {
            try
            {
                DateTimeOffset currentTime = TimeConverter.GetCurrentVietNamTime();
                if (toDate <= fromDate || fromDate > currentTime || toDate > currentTime)
                {
                    return new ServiceResponse()
                    .SetSucceeded(false)
                    .SetStatusCode(StatusCodes.Status400BadRequest)
                    .AddDetail("message", "Lấy dữ liệu giá vốn thất bại!")
                    .AddError("invalidCredentials", "Ngày bắt đầu và ngày kết thúc không hợp lệ!");
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
                foreach(var packageLab in listPackageOrder)
                {
                    var labId = await _unitOfWork.PackageLabRepository.GetByPackageId(packageLab.PackageId);
                    PackageLabDTO packageLabDTO = new PackageLabDTO()
                    {
                        Id = labId,
                        Quantity = packageLab.PackageQuantity,
                    };
                    listLabInPackage!.Add(packageLabDTO);
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
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy dữ liệu giá vốn thành công!")
                    .AddDetail("data", new { total });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .SetStatusCode(StatusCodes.Status500InternalServerError)
                    .AddDetail("message", "Lấy dữ liệu giá vốn thất bại!")
                    .AddError("outOfService", "Không thể lấy dữ liệu giá vốn ngay lúc này!");
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

    }
}