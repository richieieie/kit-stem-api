using kit_stem_api.Models.DTO;

namespace kit_stem_api.Services.IServices
{
    public interface IComponentTypeService
    {
        Task<ServiceResponse> GetComponentTypes();
        Task<ServiceResponse> CreateComponentTypeAsync(ComponentTypeCreateDTO componentTypeCreateDTO);
        Task<ServiceResponse> UpdateComponentTypeAsync(int Id, ComponentTypeUpdateDTO componentTypeUpdateDTO);
        Task<ServiceResponse> DeleteComponentTypeAsync(int Id);
    }
}
