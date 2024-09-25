using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Services.IServices;
using kit_stem_api.Repositories;
using kit_stem_api.Constants;
using System.Linq.Expressions;

namespace kit_stem_api.Services
{
    public class LabService : ILabService
    {
        private readonly int sizePerPage = 20;
        private readonly UnitOfWork _unitOfWork;
        public LabService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceResponse> CreateAsync(LabUploadDTO labUploadDTO, Guid id, string url)
        {
            try
            {
                var lab = new Lab()
                {
                    Id = id,
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
        public async Task<ServiceResponse> UpdateAsync(LabUpdateDTO labUpdateDTO, string? url)
        {
            try
            {
                var lab = await _unitOfWork.LabRepository.GetByIdAsync(labUpdateDTO.Id);
                if (lab != null)
                {
                    lab.LevelId = labUpdateDTO.LevelId;
                    lab.KitId = labUpdateDTO.KitId;
                    lab.Name = labUpdateDTO.Name!;
                    lab.Status = labUpdateDTO.Status;
                    lab.Author = labUpdateDTO.Author;
                    lab.Price = labUpdateDTO.Price;
                    lab.MaxSupportTimes = labUpdateDTO.MaxSupportTimes;
                    if (url != null)
                    {
                        lab.Url = url;
                    }

                    await _unitOfWork.LabRepository.UpdateAsync(lab);
                }

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
        public async Task<ServiceResponse> GetAsync(LabGetDTO labGetDTO)
        {
            try
            {
                Expression<Func<Lab, bool>> filter = (l) => l.Kit.Name.Contains(labGetDTO.KitName ?? "") && l.Name.Contains(labGetDTO.LabName ?? "");
                var (labs, totalPages) = await _unitOfWork.LabRepository.GetFilterAsync(filter, null, skip: sizePerPage * labGetDTO.Page, take: sizePerPage, l => l.Kit, l => l.Level, l => l.Kit.Category);

                return new ServiceResponse()
                            .AddDetail("message", "Lấy thông tin các bài lab thành công!")
                            .AddDetail("data", new { totalPages, currentPage = labGetDTO.Page + 1, labs });
            }
            catch
            {
                return new ServiceResponse()
                        .AddDetail("message", "Lấy thông tin các bài lab thành công!")
                        .AddError("outOfService", "Không thể lấy được thông tin các bài lab hiện tại hoặc vui lòng kiểm tra lại thông tin!");
            }
        }
    }
}