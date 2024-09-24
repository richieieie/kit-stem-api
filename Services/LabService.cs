using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories.IRepositories;
using kit_stem_api.Services.IServices;

namespace kit_stem_api.Services
{
    public class LabService : ILabService
    {
        private readonly ILabRepository _labRepository;
        public LabService(ILabRepository labRepository)
        {
            _labRepository = labRepository;
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
                await _labRepository.CreateAsync(lab);

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
    }
}