using AutoMapper;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Models.DTO.Response;
using KSH.Api.Repositories;
using KSH.Api.Services.IServices;
using MailKit.Net.Imap;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KSH.Api.Services
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
        #region Service methods
        public async Task<ServiceResponse> GetAsync(KitGetDTO kitGetDTO)
        {
            try
            {
                if (kitGetDTO.FromPrice > kitGetDTO.ToPrice)
                {
                    return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .AddError("invalidCredentials", "Lỗi nhập tìm kiếm")
                                    .AddDetail("message", "Không tìm thấy bộ kits");
                }
                bool package = false;
                if (kitGetDTO.FromPrice == 0 && kitGetDTO.ToPrice == int.MaxValue) { package = !package; }
                var filter = GetFilter(kitGetDTO, package);
                var (kits, totalPages) = await _unitOfWork.KitRepository.GetFilterAsync(
                    filter,
                    null,
                    skip: sizePerPage * kitGetDTO.Page,
                    take: sizePerPage,
                    query => query
                                .Include(l => l.Category)
                                .Include(l => l.KitImages)
                                .Include(l => l.KitComponents!)
                                    .ThenInclude(k => k.Component)
                    );
                if (kits == null || kits.Count() <= 0)
                {
                    return new ServiceResponse()
                                   .SetSucceeded(true)
                                   .AddDetail("message", "Không tìm thấy bộ kit!!!!!")
                                   .AddDetail("data", new { totalPages, currentPage = kitGetDTO.Page, kits = kits });
                }
                var kitsDTO = _mapper.Map<IEnumerable<KitResponseDTO>>(kits);
                
                for (int i = 0; i < kits.Count(); i ++)
                {
                    if (kits.ElementAt(i).KitComponents!.Count() > 0)
                    {
                        for (int j = 0; j < kits.ElementAt(i).KitComponents!.Count(); j++)
                        {
                            var component = new KitComponentInKitDTO()
                            {
                                ComponentId = kits.ElementAt(i).KitComponents!.ElementAt(j).ComponentId,
                                ComponentName = kits.ElementAt(i).KitComponents!.ElementAt(j).Component.Name,
                                ComponentQuantity = kits.ElementAt(i).KitComponents!.ElementAt(j).ComponentQuantity
                            };
                            kitsDTO.ElementAt(i).Components.Add(component);
                        }
                    }
                }
                return new ServiceResponse()
                                 .SetSucceeded(true)
                                 .AddDetail("message", "Lấy danh sách kit thành công")
                                 .AddDetail("data", new { totalPages, currentPage = kitGetDTO.Page + 1, kits = kitsDTO });
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
                //var kit = await _unitOfWork.KitRepository.GetByKitIdAsync(id);
                Expression<Func<Kit, bool>> filter = l => l.Id == id;
                var (kits, totalPages) = await _unitOfWork.KitRepository.GetFilterAsync(
                    filter,
                    null,
                    null,
                    null,
                    query => query
                                .Include(kc => kc.Category)
                                .Include(kc => kc.KitImages)
                                .Include(kc => kc.KitComponents!)
                                    .ThenInclude(kc => kc.Component)
                    );
                if (kits == null)
                {
                    return new ServiceResponse()
                                    .SetSucceeded(true)
                                    .AddDetail("message", "Không tìm thấy Kit")
                                    .AddDetail("data", new { kit = kits });
                }

                var kitsDTO = _mapper.Map<IEnumerable<KitResponseByIdDTO>>(kits);
                var kit = kits.FirstOrDefault();
                var kitDTO = kitsDTO.FirstOrDefault();
                if (kit!.KitComponents!.Count > 0)
                {
                    for (int i = 0; i < kit.KitComponents.Count(); i++)
                    { 
                        var conponent = new KitComponentInKitDTO()
                        {
                            ComponentId = kit.KitComponents.ElementAt(i).Component.Id,
                            ComponentName = kit.KitComponents.ElementAt(i).Component.Name,
                            ComponentQuantity = kit.KitComponents.ElementAt(i).ComponentQuantity
                        };
                        kitDTO!.Components.Add(conponent);
                    }
                } 
                return new ServiceResponse()
                                .SetSucceeded(true)
                                .AddDetail("message", "Lấy kit thành công")
                                .AddDetail("data", new { kit = kitDTO });
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
                if (await _unitOfWork.CategoryRepository.GetByIdAsync(DTO.CategoryId) == null)
                {
                    return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .AddDetail("message", "Tạo mới kit thất bại!")
                                    .AddError("invalidCredentials", $"Không tồn tại Category với Id: {DTO.CategoryId}!");
                }
                var kit = _mapper.Map<Kit>(DTO);
                var kitId = await _unitOfWork.KitRepository.CreateReturnIdAsync(kit);
                for (int i = 0; i < DTO.ComponentQuantity!.Count; i++)
                {
                    if (await _unitOfWork.ComponentRepository.GetByIdAsync(DTO.ComponentId![i]) == null)
                    {
                        return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .AddDetail("message", "Tạo mới kit thất bại!")
                                    .AddError("invalidCredentials", $"Không tồn tại Category với Id: {DTO.ComponentId[i]}!");
                    }
                    var composnent = new KitComponent()
                    {
                        KitId = kitId,
                        ComponentId = DTO.ComponentId![i],
                        ComponentQuantity = DTO.ComponentQuantity[i]
                    };
                    await _unitOfWork.KitComponentRepository.CreateAsync(composnent);
                }
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
                if ((DTO.ComponentId != null && DTO.ComponentQuantity != null && DTO.ComponentId.Count != DTO.ComponentQuantity.Count) || 
                    (DTO.ComponentId != null && DTO.ComponentQuantity == null) ||
                    (DTO.ComponentId == null && DTO.ComponentQuantity != null))
                {
                    return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .AddDetail("message", "Cập nhật kit thất bại!")
                                    .AddError("error", "Lỗi lập dữ liệu không hợp lệ")
                                    .AddError("outOfService", "Không thể tạo mới kit ngay lúc này!");
                }
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
                kit.Description = DTO.Description!;
                kit.PurchaseCost = DTO.PurchaseCost;
                kit.Status = true;
                await _unitOfWork.KitRepository.UpdateAsync(kit);
                // Update table Kit conponent
                
                if (! await _unitOfWork.KitComponentRepository.DeleteAsync(kit.Id))
                {
                    return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .AddDetail("message", "Cập nhật kit thất bại")
                                    .AddError("error", "Lỗi không thể xóa Kit support")
                                    .AddError("outOfService", "Không thể cập nhật kit ngay lúc này!");
                }
                if (DTO.ComponentId != null && DTO.ComponentQuantity != null)
                {
                    for (int i = 0; i < DTO.ComponentQuantity!.Count; i++)
                    {
                        var composnent = new KitComponent()
                        {
                            KitId = kit.Id,
                            ComponentId = DTO.ComponentId![i],
                            ComponentQuantity = DTO.ComponentQuantity[i]
                        };
                    
                        await _unitOfWork.KitComponentRepository.CreateAsync(composnent);
                    }
                }
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
        public async Task<ServiceResponse> GetPackagesByKitId(PackageGetByKitIdDTO DTO)
        {
            try
            {
                var kit = await _unitOfWork.KitRepository.GetByIdAsync(DTO.KitId);
                if (kit == null || !kit.Status)
                {
                    return new ServiceResponse()
                                    .SetSucceeded(false)
                                    .AddDetail("message", "Lấy Packages không thành công")
                                    .AddError("notFound", $"Không tồn tại Kit với Id {DTO.KitId}");
                }

                var (packages, totalPages) = await _unitOfWork.PackageRepository.GetFilterAsync(l => (l.KitId == DTO.KitId) && l.Status == DTO.Status, null, null, null, true);
                var packagesDTO = _mapper.Map<IEnumerable<PackageInKitDTO>>(packages);
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
                var labDTOs = _mapper.Map<IEnumerable<LabInKitDTO>>(labs);
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
        #endregion
        #region Methods that help service
        private Expression<Func<Kit, bool>> GetFilter(KitGetDTO kitGetDTO, bool package)
        {
            string kitName = kitGetDTO.KitName.ToLower();
            string categoryName = kitGetDTO.CategoryName.ToLower();
            int levelId = kitGetDTO.LevelId;

            if (package)
            {
                return kit => kit.Name.ToLower().Contains(kitName) &&
                              kit.Category.Name.ToLower().Contains(categoryName) &&
                              (levelId == 0 || kit.Labs!.Any(lab => lab.LevelId == levelId)) &&
                              kit.Status == kitGetDTO.Status;
            }

            return kit => kit.Name.ToLower().Contains(kitName) &&
                          kit.Category.Name.ToLower().Contains(categoryName) &&
                          kit.Packages!.Any(package => package.Price >= kitGetDTO.FromPrice && package.Price <= kitGetDTO.ToPrice) && 
                          (levelId == 0 || kit.Labs!.Any(lab => lab.LevelId == levelId)) &&
                          kit.Status == kitGetDTO.Status;
        }
        #endregion
    }
}
