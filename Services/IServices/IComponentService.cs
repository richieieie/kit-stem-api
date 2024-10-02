using kit_stem_api.Models.DTO;

namespace kit_stem_api.Services.IServices
{
    public interface IComponentService
    {
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> GetByIdAsync(int id);
        Task<ServiceResponse> CreateAsync(ComponentCreateDTO component);
        Task<ServiceResponse> UpdateAsync(ComponentUpdateDTO component);
        Task<ServiceResponse> RemoveByIdAsync(int id);
        Task<ServiceResponse> RestoreByIdAsync(int id);

    }
}
