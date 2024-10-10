using AutoMapper;
using KST.Api.Models.Domain;
using KST.Api.Models.DTO.Request;
using KST.Api.Repositories;
using KST.Api.Services.IServices;
using System.Linq.Expressions;

namespace KST.Api.Services
{
    public class KitImageService : IKitImageService
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public KitImageService(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResponse> CreateAsync(Guid id, int kitId, String url)
        {
            try
            {
                var kitImage = new KitImage();
                kitImage.KitId = kitId;
                kitImage.Id = id;
                kitImage.Url = url;

                await _unitOfWork.KitImageRepository.CreateAsync(kitImage);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Thêm ảnh thành công");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddError("outOfService", "Không thể tạo kit image ngay lúc này!")
                    .AddDetail("message", "Tạo ảnh thất bại");
            }
        }

        public Task<ServiceResponse> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse> RemoveAsync(int kitId)
        {
            try
            {
                Expression<Func<KitImage, bool>> filter = (l) => l.KitId == kitId;
                var (images, totalPages) = await _unitOfWork.KitImageRepository.GetFilterAsync(filter, null, null, null, null);
                foreach (var image in images)
                {
                    if (!await _unitOfWork.KitImageRepository.RemoveAsync(image))
                        return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddError("outOfService", "Không thể tạo kit image ngay lúc này!")
                            .AddDetail("message", "Xóa KitImage thất bại");
                }
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "xóa kitimage thành công");
            }
            catch
            {
                return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddError("outOfService", "Không thể tạo kit image ngay lúc này!")
                            .AddDetail("message", "Xóa KitImage thất bại");
            }


        }

        public async Task<ServiceResponse> UpdateAsync(Guid id, int kitId, string url)
        {
            try
            {
                var kitImage = new KitImage();
                kitImage.KitId = kitId;
                kitImage.Id = id;
                kitImage.Url = url;

                await _unitOfWork.KitImageRepository.UpdateAsync(kitImage);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Thêm ảnh thành công");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddError("outOfService", "Không thể tạo kit image ngay lúc này!")
                    .AddDetail("message", "Tạo ảnh thất bại");
            }
        }
    }
}
