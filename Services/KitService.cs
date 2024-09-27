using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories;
using kit_stem_api.Services.IServices;

namespace kit_stem_api.Services
{
    public class KitService : IKitService
    {
        private readonly UnitOfWork _unitOfWork;

        public KitService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResponse> GetAsync()
        {
            try
            {
                var kit = await _unitOfWork.KitRepository.GetAllAsync();
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "lấy danh sach kit thành công")
                    .AddDetail("data", new { kit });
            } 
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "lấy danh sach kit không thành công")
                    .AddError("outOfService", "Không thể lấy danh sách kit ngay lúc này!");
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
                    .AddDetail("message", "tạo mới kit thất bại!")
                    .AddError("outOfService", "không thể tạo mới kit ngay lúc này!");
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
                    PurchaseCost = DTO.PurchaseCost
                };
                await _unitOfWork.KitRepository.UpdateAsync(kit);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "cập nhật kit thành công");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "cập nhật kit thất bại")
                    .AddError("outOfService", "không thể cập nhật kit ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> DeleteAsync(int id)
        {
            try
            {
                var kit = new Kit()
                {
                    Id = id,
                    Status = false
                };
                await _unitOfWork.KitRepository.RemoveAsync(kit);
                return new ServiceResponse().SetSucceeded(true)
                    .SetSucceeded(true)
                    .AddDetail("message", "xóa kit thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "xóa kit thất bại")
                    .AddError("outOfService", "không thể xóa kit ngay lúc này!");
            }
        }
    }
}
