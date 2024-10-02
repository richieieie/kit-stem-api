using AutoMapper;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Repositories;
using kit_stem_api.Services.IServices;
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
                Expression<Func<Kit, bool>> filter = (l) => l.Name.Contains(kitGetDTO.KitName ?? "") && l.Category.Name.Contains(kitGetDTO.CategoryName ?? "");
                var (Kits, totalPages) = await _unitOfWork.KitRepository.GetFilterAsync(
                    filter,
                    null,
                    skip: sizePerPage * kitGetDTO.Page,
                    take: sizePerPage,
                    query => query.Include(l => l.Category)
                    );
                var kitsDTO = _mapper.Map<IEnumerable<Kit>>(Kits);

                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy danh sách kit thành công")
                    .AddDetail("data", new { totalPages, currcurrentPage = kitGetDTO.Page + 1, kits = kitsDTO });
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
                    query => query.Include(l => l.Category)
                    );
                var kitsDTO = _mapper.Map<IEnumerable<Kit>>(Kits);

                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy kit thành công")
                    .AddDetail("data", new { kitsDTO });
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
                var newKit = new Kit()
                {
                    CategoryId = DTO.CategoryId,
                    Name = DTO.Name,
                    Brief = DTO.Brief,
                    Description = DTO.Description,
                    PurchaseCost = DTO.PurchaseCost,
                    Status = true
                };
                await _unitOfWork.KitRepository.CreateAsync(newKit);
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
                var kit = new Kit()
                {
                    Id = DTO.Id,
                    CategoryId = DTO.CategoryId,
                    Name = DTO.Name,
                    Brief = DTO.Brief,
                    Description = DTO.Description,
                    PurchaseCost = DTO.PurchaseCost,
                    Status = true
                };
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
                        .AddDetail("message", "Không tìm thấy kit!");

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
    }
}
