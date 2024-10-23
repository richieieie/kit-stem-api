using AutoMapper;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Repositories;
using KSH.Api.Services.IServices;
using System.Linq.Expressions;

namespace KSH.Api.Services
{
    public class ComponentService : IComponentService
    {
        private readonly int sizePerPage = 20;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ComponentService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> CreateAsync(ComponentCreateDTO component)
        {
            try
            {
                var newComponent = new Component()
                {
                    TypeId = component.TypeId,
                    Name = component.Name,
                    Status = true,
                };
                await _unitOfWork.ComponentRepository.CreateAsync(newComponent);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("message", "Tạo mới linh kiện thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Tạo mới linh kiện thất bại!")
                    .AddError("outOfService", "Không thể tạo mới linh kiện ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> RemoveByIdAsync(int id)
        {
            try
            {
                var component = await _unitOfWork.ComponentRepository.GetByIdAsync(id);
                if (component == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status404NotFound)
                        .AddDetail("message", "Xóa linh kiện thất bại!")
                        .AddError("notFound", "Không tìm thấy linh kiện!");
                }
                component.Status = false;
                await _unitOfWork.ComponentRepository.UpdateAsync(component);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("message", "Xóa linh kiện thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Xóa linh kiện thất bại!")
                    .AddError("outOfService", "Không thể xóa linh kiện ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> RestoreByIdAsync(int id)
        {
            try
            {
                var component = await _unitOfWork.ComponentRepository.GetByIdAsync(id);
                if (component == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status404NotFound)
                        .AddDetail("message", "Phục hồi linh kiện thất bại!")
                        .AddError("notFound", "Không tìm thấy linh kiện!");
                }
                component.Status = true;
                await _unitOfWork.ComponentRepository.UpdateAsync(component);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("message", "Phục hồi linh kiện thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Phục hồi linh kiện thất bại!")
                    .AddError("outOfService", "Không thể Phục hồi linh kiện ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetAsync(ComponentGetDTO  componentGetDTO)
        {
            try
            {
                Expression<Func<Component, bool>> filter = GetFilter(componentGetDTO);
                var (componentModels, totalPages) = await _unitOfWork.ComponentRepository.GetFilterAsync(filter, null, sizePerPage * componentGetDTO.Page, sizePerPage);
                
                var components = _mapper.Map<IEnumerable<ComponentDTO>>(componentModels);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy danh sách linh kiện thành công!")
                    .AddDetail("data", new { totalPages, currentPage = componentGetDTO.Page, componentModels = components });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy danh sách thành phần thất bại!")
                    .AddError("outOfService", "Không thể lấy danh sách thành phần ngày lúc này!");
            }
        }

        public async Task<ServiceResponse> UpdateAsync(ComponentUpdateDTO component)
        {
            try
            {

                var updateComponent = await _unitOfWork.ComponentRepository.GetByIdAsync(component.Id);
                if (updateComponent == null)
                {
                    return new ServiceResponse()
                       .SetSucceeded(false)
                       .SetStatusCode(StatusCodes.Status404NotFound)
                       .AddDetail("message", "Chỉnh sửa linh kiện thất bại!")
                       .AddError("notFound", "Không tìm thấy linh kiện!");
                }

                updateComponent.Id = component.Id;
                updateComponent.TypeId = component.TypeId;
                updateComponent.Name = component.Name;
                updateComponent.Status = true;

                await _unitOfWork.ComponentRepository.UpdateAsync(updateComponent);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Chỉnh sửa linh kiện thành công!");
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Chỉnh sửa linh kiện thất bại!")
                        .AddError("outOfSercive", "Không thể chỉnh sửa linh kiện ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetByIdAsync(int id)
        {
            try
            {
                var componentModel = await _unitOfWork.ComponentRepository.GetByIdAsync(id);
                if (componentModel == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status404NotFound)
                        .AddDetail("message", "Lấy thông tin linh kiện thất bại!")
                        .AddError("notFound", "Không tìm thấy linh kiện!");
                }
                var component = _mapper.Map<Component, ComponentDTO>(componentModel);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("data", new { component });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy thông tin linh kiện thất bại")
                    .AddError("outOfService", "Không thể lấy thông tin linh kiện ngay lúc này!");
            }
        }

        private Expression<Func<Component, bool>> GetFilter(ComponentGetDTO componentGetDTO)
        {
            return (c) => c.Name.Contains(componentGetDTO.Name ?? "");
        }
    }
}
