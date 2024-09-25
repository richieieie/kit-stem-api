using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Services.IServices;
using kit_stem_api.Repositories;

namespace kit_stem_api.Services
{
    public class LabService : ILabService
    {
        private readonly UnitOfWork _unitOfWork;
        public LabService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResponse> CreateAsync(LabUploadDTO labUploadDTO, string url)
        {
            try
            {
                var lab = new Lab()
                {
                    Id = Guid.NewGuid(),
                    LevelId = labUploadDTO.LevelId,
                    KitId = labUploadDTO.KitId,
                    Name = labUploadDTO.Name!,
                    Url = url,
                    Status = labUploadDTO.Status,
                    Author = labUploadDTO.Author,
                    Price = labUploadDTO.Price,
                    MaxSupportTimes = labUploadDTO.MaxSupportTimes
                };
                await _unitOfWork.LabRepository.CreateAsync(lab);

                return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Thêm mới bài lab thành công!");
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Thêm mới bài lab thất bại!")
                        .AddError("outOfService", "Không thể tạo mới bài lab ngay lúc này");
            }
        }

        public async Task<ServiceResponse> UpdateAsync(LabUpdateDTO labUpdateDTO, string url)
        {
            try
            {
                var lab = new Lab()
                {
                    Id = labUpdateDTO.Id,
                    LevelId = labUpdateDTO.LevelId,
                    KitId = labUpdateDTO.KitId,
                    Name = labUpdateDTO.Name!,
                    Url = url,
                    Status = labUpdateDTO.Status,
                    Author = labUpdateDTO.Author,
                    Price = labUpdateDTO.Price,
                    MaxSupportTimes = labUpdateDTO.MaxSupportTimes
                };
                await _unitOfWork.LabRepository.UpdateAsync(lab);

                return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Chỉnh sửa bài lab thành công!");
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Chỉnh sửa bài lab thất bại!")
                        .AddError("outOfService", "Không thể tạo mới bài lab ngay lúc này");
            }
        }
    }
}