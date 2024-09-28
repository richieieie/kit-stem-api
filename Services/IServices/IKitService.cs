using kit_stem_api.Models.DTO;

namespace kit_stem_api.Services.IServices
{
    public interface IKitService
    {
        Task<ServiceResponse> GetAsync();
        Task<ServiceResponse> CreateAsync(KitCreateDTO DTO);
        Task<ServiceResponse> UpdateAsync(KitUpdateDTO DTO);
        Task<ServiceResponse> RemoveAsync(int id);
    }
}
