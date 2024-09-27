using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories;
using kit_stem_api.Services.IServices;
using System.Linq.Expressions;

namespace kit_stem_api.Services
{
    public class PackageService : IPackageService
    {
        private readonly int sizePerPage = 20;
        private readonly UnitOfWork _unitOfWork;
        public PackageService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResponse> GetAsync(PackageGetDTO packageGetDTO)
        {
            try
            {
                Expression<Func<Package, bool>> filter = (p) =>
                                       (packageGetDTO.LevelId == 0 || p.LevelId == packageGetDTO.LevelId) &&
                                       (p.Price >= packageGetDTO.FromPrice) &&
                                       (p.Price <= packageGetDTO.ToPrice) &&
                                       p.Kit.Name.Contains(packageGetDTO.KitName ?? "") &&
                                       p.Kit.Category.Name.Contains(packageGetDTO.CategoryName ?? "") &&
                                       p.Status == packageGetDTO.Status;
                var (packages, totalPages) = await _unitOfWork
                                                .PackageRepository
                                                .GetFilterAsync(
                                                                filter,
                                                                null,
                                                                skip: sizePerPage * packageGetDTO.Page,
                                                                take: sizePerPage,
                                                                p => p.Kit,
                                                                p => p.Level,
                                                                p => p.Kit.Category,
                                                                 p => p.PackageLabs
                                                               );
                return new ServiceResponse()
                                .AddDetail("message", "Lấy thông tin các gói kit thành công!")
                                .AddDetail("data", new { totalPages, currentPage = packageGetDTO.Page + 1, packages });
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
