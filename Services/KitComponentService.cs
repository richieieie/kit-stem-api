using AutoMapper;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Repositories;
using kit_stem_api.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Services
{
    public class KitComponentService : IKitComponentService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public KitComponentService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> CreateAsync(KitComponentDTO kitComponentDTO)
        {
            try
            {
               
                var kitComponent = _mapper.Map<KitComponent>(kitComponentDTO);

                var component = await _unitOfWork.ComponentRepository.GetByIdAsync(kitComponent.ComponentId);
                if (component == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Thêm linh kiện cho kit thất bại!")
                        .AddError("notFound", "Không tìm thấy linh kiện cho kit ngay lúc này!");
                }

                await _unitOfWork.KitComponentRepository.CreateAsync(kitComponent);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Thêm linh kiện cho kit thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Thêm linh kiện cho kit thất bại!")
                    .AddError("outOfService", "Không thể thêm linh kiện cho kit ngay lúc này!");
            }

        }

        public async Task<ServiceResponse> GetAllAsync()
        {
            try
            {
                var kitComponents = await _unitOfWork.KitComponentRepository.GetAllAsync();
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy danh sách linh kiện kit thành công!")
                    .AddDetail("data", new { kitComponents });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy danh sách linh kiện kit thất bại!")
                    .AddError("outOfService", "Không thể lấy danh sách linh kiện kit ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetByKitIdAsync(int kitId)
        {
            try
            {
                var (kitComponents, totalPages) = await _unitOfWork.KitComponentRepository.GetFilterAsync(
                    kc => kc.KitId == kitId,
                    includes: new Func<IQueryable<KitComponent>, IQueryable<KitComponent>>[]
                    {
                        kc => kc.Include(k => k.Component)
                    }
                );

                if (!kitComponents.Any())
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Lấy danh sách linh kiện cho kit thất bại!")
                        .AddError("notFound", "Không tìm thấy linh kiện cho kit này!");
                }

                var componentDTO = _mapper.Map<IEnumerable<KitComponentDTO>>(kitComponents);

                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy danh sách linh kiện thành công!")
                    .AddDetail("data", new { kitComponents = componentDTO });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy danh sách linh kiện thất bại!")
                    .AddError("outOfService", "Không thể lấy danh sách linh kiện ngay lúc này!");
            }
        }


        public async Task<ServiceResponse> UpdateAsync(KitComponentDTO kitComponentDTO)
        {
            try
            {
                var kitComponent = _mapper.Map<KitComponent>(kitComponentDTO);
                var component = await _unitOfWork.ComponentRepository.GetByIdAsync(kitComponent.ComponentId);
                if (component == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Chỉnh sửa linh kiện cho kit thất bại!")
                        .AddError("notFound", "Không tìm thấy linh kiện cho kit ngay lúc này!");
                }

                await _unitOfWork.KitComponentRepository.UpdateAsync(kitComponent);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Chỉnh sửa linh kiện cho kit thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Chỉnh sửa linh kiện cho kit thất bại!")
                    .AddError("outOfService", "Không thể Chỉnh sửa linh kiện cho kit ngay lúc này!");
            }
        }
    }
}
