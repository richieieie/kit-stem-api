using AutoMapper;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Models.DTO.Response;
using kit_stem_api.Repositories;
using kit_stem_api.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace kit_stem_api.Services
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

        #region Services methods
        public async Task<(ServiceResponse, int)> CreateAsync(PackageCreateDTO packageCreateDTO)
        {
            using var transaction = await _unitOfWork._dbContext.Database.BeginTransactionAsync();
            try
            {
                var package = _mapper.Map<Package>(packageCreateDTO);

                // Add package
                await _unitOfWork.PackageRepository.CreateAsync(package);

                // If the package contains Labs, then try to add those labs in PackageLab table
                if (!await TryCreatePackageLabsAsync(packageCreateDTO, package))
                {
                    await transaction.RollbackAsync();
                    return (new ServiceResponse()
                            .SetSucceeded(false)
                            .SetStatusCode(StatusCodes.Status400BadRequest)
                            .AddDetail("message", "Thêm mới gói kit thất bại!")
                            .AddError("invalidCredentials", "Không thể thêm mới gói kit do thông tin yêu cầu không chính xác!"),
                            0);
                }

                await transaction.CommitAsync();

                return (new ServiceResponse()
                        .AddDetail("message", "Thêm mới gói kit thành công!"),
                        package.Id);
            }
            catch
            {
                await transaction.RollbackAsync();
                return (new ServiceResponse()
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
                // Construct filter for using in Where()
                Expression<Func<Package, bool>> filter = GetFilter(packageGetFilterDTO);

                // Try to get an IEnumerable<Package> and total pages
                var (packages, totalPages) = await _unitOfWork.PackageRepository.GetFilterAsync(filter, null, skip: sizePerPage * packageGetFilterDTO.Page, take: sizePerPage);

                // Map IEnumerable<Package> to IEnumerable<PackageResponseDTO> using AutoMapper
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
                // Construct filter for using in Where()
                var package = await _unitOfWork.PackageRepository.GetByIdAsync(id);

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
                        .AddDetail("message", "Lấy thông tin gói kit thành công!")
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
                        .AddDetail("message", "Xoá gói kit thất bại!")
                        .AddError("outOfService", "Hiện tại không thể xoá được gói kit!");
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
        private async Task<bool> TryCreatePackageLabsAsync(PackageCreateDTO packageCreateDTO, Package package)
        {
            if (packageCreateDTO.LabIds == null)
            {
                return true;
            }

            var (labs, _) = await _unitOfWork.LabRepository.GetByKitIdAsync(packageCreateDTO.KitId);
            var validLabIds = labs.Select(l => l.Id);
            if (packageCreateDTO.LabIds.All(id => validLabIds.Contains(id)))
            {
                var packageLabs = packageCreateDTO.LabIds.Select(labId => new PackageLab() { PackageId = package.Id, LabId = labId });

                await _unitOfWork.PackageLabRepository.CreateAsync(packageLabs);

                return true;
            }

            return false;
        }
        #endregion
    }
}
