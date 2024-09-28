using AutoMapper;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Models.DTO.Response;
using kit_stem_api.Repositories;
using kit_stem_api.Services.IServices;
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

        #region Methods that are constructing arguments for PackageRepository
        private Expression<Func<Package, bool>> GetFilter(PackageGetFilterDTO packageGetFilterDTO)
        {
            return (p) => (packageGetFilterDTO.LevelId == 0 || p.LevelId == packageGetFilterDTO.LevelId) &&
                            (p.Price >= packageGetFilterDTO.FromPrice) &&
                            (p.Price <= packageGetFilterDTO.ToPrice) &&
                            p.Kit.Name.Contains(packageGetFilterDTO.KitName ?? "") &&
                            p.Kit.Category.Name.Contains(packageGetFilterDTO.CategoryName ?? "") &&
                            p.Status == packageGetFilterDTO.Status;
        }
        #endregion
    }
}
