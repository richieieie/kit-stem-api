using AutoMapper;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Models.DTO.Response;
using kit_stem_api.Repositories;
using kit_stem_api.Services.IServices;
using MailKit.Net.Imap;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace kit_stem_api.Services
{
    public class KitService : IKitService
    {
        private readonly int sizePerPage = 20;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public KitService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ServiceResponse> GetAsync(KitGetDTO kitGetDTO)
        {
            try
            {
                var filter = GetFilter(kitGetDTO);

                var (Kits, totalPages) = await _unitOfWork.KitRepository.GetFilterAsync(
                    filter,
                    null,
                    skip: sizePerPage * kitGetDTO.Page,
                    take: sizePerPage,
                    query => query.Include(l => l.Category).Include(l => l.KitImages)
                    );
                if (Kits.Count() > 0)
                {
                    var kitsDTO = _mapper.Map<IEnumerable<KitResponseDTO>>(Kits);
                    return new ServiceResponse()
                         .SetSucceeded(true)
                         .AddDetail("message", "Lấy danh sách kit thành công")
                         .AddDetail("data", new { totalPages, currentPage = (kitGetDTO.Page + 1), kits = kitsDTO });
                }
                else
                {
                    return new ServiceResponse()
                       .SetSucceeded(true)
                       .AddDetail("message", "Không tìm thấy bộ kit!!!!!");
                }
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy danh sách kit không thành công")
                    .AddError("outOfService", "Không thể lấy danh sách kit ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            try
            {
                Expression<Func<Kit, bool>> filter = (l) => l.Id.Equals(id);
                var (Kits, totalPages) = await _unitOfWork.KitRepository.GetFilterAsync(
                    filter,
                    null,
                    null,
                    null,
                    query => query.Include(l => l.Category).Include(l => l.KitImages)
                    );

                if (Kits.FirstOrDefault() == null || !Kits.FirstOrDefault().Status)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy kit không thành công")
                        .AddError("notFound", "Không tìm thấy kit");
                }

                var kitsDTO = _mapper.Map<IEnumerable<KitResponseDTO>>(Kits);
                

                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy kit thành công")
                    .AddDetail("data", new { kit = kitsDTO });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy kit không thành công")
                    .AddError("outOfService", "Không thể lấy kit ngay lúc này!");
            }
        }



        public async Task<ServiceResponse> CreateAsync(KitCreateDTO DTO)
        {
            try
            {
                var kit = _mapper.Map<Kit>(DTO);
                var kitId = await _unitOfWork.KitRepository.CreateAsync(kit);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Tạo mới kit thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Tạo mới kit thất bại!")
                    .AddError("outOfService", "Không thể tạo mới kit ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> UpdateAsync(KitUpdateDTO DTO)
        {
            try
            {
                var kit = await _unitOfWork.KitRepository.GetByIdAsync(DTO.Id);
                if (kit == null || !kit.Status)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Không thể cập nhật kit")
                        .AddError("notFound", "Không tìm thấy kit");
                }
                
                kit.CategoryId = DTO.CategoryId;
                kit.Name = DTO.Name;
                kit.Brief = DTO.Brief;
                kit.Description = DTO.Description;
                kit.PurchaseCost = DTO.PurchaseCost;
                kit.Status = true;
                await _unitOfWork.KitRepository.UpdateAsync(kit);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Cập nhật kit thành công");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Cập nhật kit thất bại")
                    .AddError("outOfService", "Không thể cập nhật kit ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> RemoveAsync(int id)
        {
            try
            {
                var kit = await _unitOfWork.KitRepository.GetByIdAsync(id);
                if (kit == null)
                    return new ServiceResponse().SetSucceeded(false)
                        .AddDetail("message", "Không tìm thấy kit!")
                        .AddError("notFound", "Không tìm thấy kit"); ;

                kit.Status = false;

                await _unitOfWork.KitRepository.UpdateAsync(kit);
                return new ServiceResponse().SetSucceeded(true)
                    .SetSucceeded(true)
                    .AddDetail("message", "Xóa kit thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Xóa kit thất bại")
                    .AddError("outOfService", "Không thể xóa kit ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> RestoreByIdAsync(int id)
        {
            try
            {
                var kit = await _unitOfWork.KitRepository.GetByIdAsync(id);
                if (kit == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddError("notFound", "Không tìm thấy kit")
                        .AddDetail("message", "Khôi phục kit thất bại!");
                }
                kit.Status = true;
                await _unitOfWork.KitRepository.UpdateAsync(kit);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Khôi phục kit thành công");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddError("outOfService", "Không thể khôi phục ngay lúc này")
                    .AddDetail("message", "Khôi phục kit thất bại");
            }
        }

        public async Task<ServiceResponse> GetPackagesByKitId(int id)
        {
            try
            {
                var kit = await _unitOfWork.KitRepository.GetByIdAsync(id);
                if (kit == null || !kit.Status)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy Packages không thành công")
                        .AddError("notFound", $"Không tồn tại Kit với Id {id}");
                }

                var (packages, totalPages) = await _unitOfWork.PackageRepository.GetFilterAsync((l) => (l.KitId == id), null, null, null, true);
                var packagesDTO = _mapper.Map<IEnumerable<PackageResponseDTO>>(packages);
                return new ServiceResponse()
                            .AddDetail("message", "Lấy thông tin Package thành công!")
                            .AddDetail("data", new { totalPages, currentPage = 0, Packages = packagesDTO });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddError("outOfService", "Không thể khôi phục ngay lúc này")
                    .AddDetail("message", "không thể lấy packet lúc này");

            }
        }

        public async Task<ServiceResponse> GetLabByKitId(int id)
        {
            try
            {
                var kit = await _unitOfWork.KitRepository.GetByIdAsync(id);
                if (kit == null || !kit.Status)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy Labs không thành công")
                        .AddError("notFound", $"Không tồn tại Kit với Id {id}");
                }

                var (labs, totalPages) = await _unitOfWork.LabRepository.GetByKitIdAsync(id);
                var labDTOs = _mapper.Map<IEnumerable<LabResponseDTO>>(labs);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy thông tin bài lab thành công!")
                    .AddDetail("data", new { totalPages, currentPage = 0, labs = labDTOs });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy thông tin bài lab không thành công!")
                        .AddError("outOfService", "Không thể lấy được thông tin bài lab hiện tại hoặc vui lòng kiểm tra lại thông tin!");
            }
        }
        public async Task<int> GetMaxIdAsync()
        {
            try
            {
                var kitId = await _unitOfWork.KitRepository.GetMaxIdAsync();
                return kitId;
            }
            catch
            {
                return -1;
            }
        }
        private Expression<Func<Kit, bool>> GetFilter(KitGetDTO kitGetDTO)
        {
            return (l) => l.Name.ToLower().Contains(kitGetDTO.KitName.ToLower()) && l.Category.Name.ToLower().Contains(kitGetDTO.CategoryName.ToLower());
        }
    }
}
