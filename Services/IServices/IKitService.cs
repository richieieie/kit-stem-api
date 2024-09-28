using kit_stem_api.Models.DTO.Request;

namespace kit_stem_api.Services.IServices
{
    public interface IKitService
    {
        Task<ServiceResponse> GetAsync(KitGetDTO kitGetDTO);
        Task<ServiceResponse> GetByIdAsync(int id);
        Task<ServiceResponse> CreateAsync(KitCreateDTO DTO);
        Task<ServiceResponse> UpdateAsync(KitUpdateDTO DTO);
        Task<ServiceResponse> RemoveAsync(int id);
    }
}
