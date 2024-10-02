using kit_stem_api.Models.DTO;

namespace kit_stem_api.Services.IServices
{
    public interface IComponentTypeService
    {
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> GetByIdAsync(int id);
        Task<ServiceResponse> CreateAsync(ComponentTypeCreateDTO componentTypeCreateDTO);
        Task<ServiceResponse> UpdateAsync(ComponentTypeUpdateDTO componentTypeUpdateDTO);
        Task<ServiceResponse> RemoveByIdAsync(int id);
        Task<ServiceResponse> RestoreByIdAsync(int id);

    }
}
