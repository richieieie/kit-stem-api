using kit_stem_api.Models.DTO;

namespace kit_stem_api.Services.IServices
{
    public interface IComponentService
    {
        Task<ServiceResponse> GetComponentsAsync();
        Task<ServiceResponse> CreateComponentAsync(ComponentCreateDTO component);
        Task<ServiceResponse> UpdateComponentAsync(int Id, ComponentUpdateDTO component);
        Task<ServiceResponse> DeleteComponentAsync(int Id);
    }
}
