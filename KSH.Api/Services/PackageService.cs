﻿using AutoMapper;
using KST.Api.Models.Domain;
using KST.Api.Models.DTO;
using KST.Api.Models.DTO.Request;
using KST.Api.Models.DTO.Response;
using KST.Api.Repositories;
using KST.Api.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KST.Api.Services
{
    public class PackageService : IPackageService
    {
        private readonly int sizePerPage = 20;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PackageService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Service methods
        public async Task<(ServiceResponse, int)> CreateAsync(PackageCreateDTO packageCreateDTO)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var package = _mapper.Map<Package>(packageCreateDTO);

                package = await TryCreatePackageLabsAsync(packageCreateDTO, package);

                if (package == null)
                {
                    return (serviceResponse
                            .SetSucceeded(false)
                            .SetStatusCode(StatusCodes.Status400BadRequest)
                            .AddDetail("message", "Thêm mới gói kit thất bại!")
                            .AddError("invalidCredentials", "Không thể thêm mới gói kit do thông tin yêu cầu không chính xác!"),
                            0);
                }

                await _unitOfWork.PackageRepository.CreateAsync(package);

                return (serviceResponse
                            .AddDetail("message", "Thêm mới gói kit thành công!"),
                            package.Id);
            }
            catch
            {
                return (serviceResponse
                        .SetSucceeded(false)
                        .AddDetail("message", "Thêm mới gói kit thất bại!")
                        .AddError("outOfService", "Không thể thêm mới gói kit vào thời điểm hiện tại hoặc do thông tin yêu cầu không chính xác!"),
                        0);
            }
        }

        public async Task<ServiceResponse> GetAsync(PackageGetFilterDTO packageGetFilterDTO)
        {
            try
            {
                Expression<Func<Package, bool>> filter = GetFilter(packageGetFilterDTO);

                var (packages, totalPages) = await _unitOfWork.PackageRepository.GetFilterAsync(filter, null, skip: sizePerPage * packageGetFilterDTO.Page, take: sizePerPage, packageGetFilterDTO.IncludeLabs);

                var packageDTOs = _mapper.Map<IEnumerable<PackageResponseDTO>>(packages);
                return new ServiceResponse()
                                .AddDetail("message", "Lấy thông tin các gói kit thành công!")
                                .AddDetail("data", new { totalPages, currentPage = packageGetFilterDTO.Page, packages = packageDTOs });
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy thông tin các gói kit thành công!")
                        .AddError("outOfService", "Không thể lấy được thông tin các gói kit hiện tại hoặc vui lòng kiểm tra lại thông tin!");
            }
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            try
            {
                var package = await _unitOfWork.PackageRepository.GetByIdAsync(id);
                if (package == null)
                {
                    return new ServiceResponse()
                                .SetSucceeded(false)
                                .SetStatusCode(StatusCodes.Status404NotFound)
                                .AddDetail("message", "Lấy thông tin gói kit thất bại!")
                                .AddError("notFound", "Không tìm gói kit này!");
                }

                // Map IEnumerable<Package> to IEnumerable<PackageResponseDTO> using AutoMapper
                var packageDTO = _mapper.Map<PackageResponseDTO>(package);
                return new ServiceResponse()
                                .AddDetail("message", "Lấy thông tin gói kit thành công!")
                                .AddDetail("data", new { package = packageDTO });
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy thông tin gói kit thất bại!")
                        .AddError("outOfService", "Không thể lấy được thông tin gói kit hiện tại hoặc vui lòng kiểm tra lại thông tin!");
            }
        }

        public async Task<ServiceResponse> RemoveByIdAsync(int id)
        {
            try
            {
                // Construct filter for using in Where()
                var package = await _unitOfWork.PackageRepository.GetByIdAsync(id);
                if (package == null)
                {
                    return new ServiceResponse()
                                .SetSucceeded(false)
                                .SetStatusCode(StatusCodes.Status404NotFound)
                                .AddDetail("message", "Xoá gói kit thất bại!")
                                .AddError("notFound", "Không tìm gói kit này!");
                }

                package.Status = false;
                await _unitOfWork.PackageRepository.UpdateAsync(package);

                var packageDTO = _mapper.Map<PackageResponseDTO>(package);
                return new ServiceResponse()
                                .AddDetail("message", "Xoá gói kit thành công!");
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Xoá gói kit thất bại!")
                        .AddError("outOfService", "Hiện tại không thể xoá được gói kit!");
            }
        }

        public async Task<ServiceResponse> RestoreByIdAsync(int id)
        {
            try
            {
                // Construct filter for using in Where()
                var package = await _unitOfWork.PackageRepository.GetByIdAsync(id);
                if (package == null)
                {
                    return new ServiceResponse()
                                .SetSucceeded(false)
                                .SetStatusCode(StatusCodes.Status404NotFound)
                                .AddDetail("message", "Khôi phục gói kit thất bại!")
                                .AddError("notFound", "Không tìm gói kit này!");
                }

                package.Status = true;
                await _unitOfWork.PackageRepository.UpdateAsync(package);

                return new ServiceResponse()
                                .AddDetail("message", "Khôi phục gói kit thành công!");
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Khôi phục gói kit thất bại!")
                        .AddError("outOfService", "Hiện tại không thể xoá được gói kit!");
            }
        }

        public async Task<ServiceResponse> UpdateAsync(PackageUpdateDTO packageUpdateDTO)
        {
            try
            {
                var package = await _unitOfWork.PackageRepository.GetByIdAsync(packageUpdateDTO.Id);
                if (package == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status404NotFound)
                        .AddDetail("message", "Cập nhật thông tin gói kit thất bại!")
                        .AddError("invalidCredentials", "Không thể tìm thấy gói Kit của bạn!");
                }

                package.Name = packageUpdateDTO.Name!;
                package.LevelId = packageUpdateDTO.LevelId!;
                package.Price = packageUpdateDTO.Price!;
                await _unitOfWork.PackageRepository.UpdateAsync(package);

                return new ServiceResponse()
                                .AddDetail("message", "Cập nhật thông tin gói kit thành công!");
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Cập nhật thông tin gói kit thất bại!")
                        .AddError("outOfService", "Không thể cập nhật thông tin gói kit hiện tại hoặc vui lòng kiểm tra lại thông tin!");
            }
        }
        #endregion

        #region Methods that help service
        private Expression<Func<Package, bool>> GetFilter(PackageGetFilterDTO packageGetFilterDTO)
        {
            return (p) => (packageGetFilterDTO.LevelId == 0 || p.LevelId == packageGetFilterDTO.LevelId) &&
                            (p.Price >= packageGetFilterDTO.FromPrice) &&
                            (p.Price <= packageGetFilterDTO.ToPrice) &&
                            p.Name.Contains(packageGetFilterDTO.Name ?? "") &&
                            p.Kit.Name.Contains(packageGetFilterDTO.KitName ?? "") &&
                            p.Kit.Category.Name.Contains(packageGetFilterDTO.CategoryName ?? "") &&
                            p.Status == packageGetFilterDTO.Status;
        }
        private async Task<Package?> TryCreatePackageLabsAsync(PackageCreateDTO packageCreateDTO, Package package)
        {
            if (packageCreateDTO.LabIds == null || packageCreateDTO.LabIds.Count == 0)
            {
                return package;
            }

            var (labs, _) = await _unitOfWork.LabRepository.GetByKitIdAsync(packageCreateDTO.KitId);
            var validLabIds = labs.Select(l => l.Id);
            if (!packageCreateDTO.LabIds.All(id => validLabIds.Contains(id)))
            {
                return null;
            }

            var packageLabs = packageCreateDTO.LabIds.Select(labId => new PackageLab() { PackageId = package.Id, LabId = labId });
            package.PackageLabs = packageLabs.ToList();

            return package;
        }
        #endregion
    }
}
