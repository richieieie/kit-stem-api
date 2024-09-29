using kit_stem_api.Models.DTO.Request;

namespace kit_stem_api.Services.IServices
{
    public interface IKitComponentService
    {
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> GetByKitIdAsync(int kitId);
        Task<ServiceResponse> CreateAsync(KitComponentDTO kitComponent);
        Task<ServiceResponse> UpdateAsync(KitComponentDTO kitComponent);
    }
}
