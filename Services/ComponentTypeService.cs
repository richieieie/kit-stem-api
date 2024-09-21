using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories;
using kit_stem_api.Repositories.IRepositories;
using kit_stem_api.Services.IServices;

namespace kit_stem_api.Services
{
    public class ComponentTypeService : IComponentTypeService
    {
        private readonly IComponentTypeRepository _componentTypeRepository;

        public ComponentTypeService(IComponentTypeRepository componentTypeRepository)
        {
            _componentTypeRepository = componentTypeRepository;
        }

        public async Task<ServiceResponse> CreateComponentTypeAsync(ComponentTypeCreateDTO componentTypeCreateDTO)
        {
            try
            {
                var newComponentType = new ComponentsType()
                {
                    Name = componentTypeCreateDTO.Name
                };
                newComponentType = await _componentTypeRepository.CreateComponentTypeAsync(newComponentType);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("newComponentType", newComponentType);
            } 
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("unhandledException", "Không thể tạo một component type ngay lúc này!");
            }
            
        }

        public async Task<ServiceResponse> DeleteComponentTypeAsync(int Id)
        {
            try
            {
                var deleteComponentType = await _componentTypeRepository.DeleteComponentTypeAsync(Id);
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("deleteComponentType", deleteComponentType);
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("unhandledException", "Không thể xóa component type ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> GetComponentTypes()
        {
            try
            {
                var componentTypes = await _componentTypeRepository.GetComponentTypesAsync();
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("componentTypes", componentTypes);
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("unhandledException", "Không thể lấy danh sách component type ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> UpdateComponentTypeAsync(int Id, ComponentTypeUpdateDTO componentTypeUpdateDTO)
        {
            try
            {
                var updateComponentType = await _componentTypeRepository.UpdateComponentTypeAsync(Id, componentTypeUpdateDTO);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("updateComponentType", updateComponentType);
            } catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("unhandledException", "Không thể update component type ngay lúc này!"); 
            }
        }
    }
}
