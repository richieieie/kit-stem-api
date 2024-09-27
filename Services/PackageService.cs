using AutoMapper;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
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
        public async Task<ServiceResponse> GetAsync(PackageGetDTO packageGetDTO)
        {
            try
            {
                // Construct filter for using in Where()
                Expression<Func<Package, bool>> filter = (p) =>
                                       (packageGetDTO.LevelId == 0 || p.LevelId == packageGetDTO.LevelId) &&
                                       (p.Price >= packageGetDTO.FromPrice) &&
                                       (p.Price <= packageGetDTO.ToPrice) &&
                                       p.Kit.Name.Contains(packageGetDTO.KitName ?? "") &&
                                       p.Kit.Category.Name.Contains(packageGetDTO.CategoryName ?? "") &&
                                       p.Status == packageGetDTO.Status;

                // Try to get an IEnumerable<Package> and total pages
                var (packages, totalPages) = await _unitOfWork
                                                .PackageRepository
                                                .GetFilterAsync(
                                                                filter,
                                                                null,
                                                                skip: sizePerPage * packageGetDTO.Page,
                                                                take: sizePerPage,
                                                                query => query.Include(p => p.Kit),
                                                                query => query.Include(p => p.Level),
                                                                query => query.Include(p => p.Kit.Category),
                                                                query => query.Include(p => p.PackageLabs)!.ThenInclude(pl => pl.Lab)
                                                               );

                // Map IEnumerable<Package> to IEnumerable<PackageResponseDTO> using AutoMapper
                var packageDTOs = _mapper.Map<IEnumerable<PackageResponseDTO>>(packages);
                return new ServiceResponse()
                                .AddDetail("message", "Lấy thông tin các gói kit thành công!")
                                .AddDetail("data", new { totalPages, currentPage = packageGetDTO.Page + 1, packages = packageDTOs });
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy thông tin các gói kit thành công!")
                        .AddError("outOfService", "Không thể lấy được thông tin các gói kit hiện tại hoặc vui lòng kiểm tra lại thông tin!");
            }
        }
    }
}
