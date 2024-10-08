using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO.Request;

namespace kit_stem_api.Services.IServices
{
    public interface IKitImageService
    {
        Task<ServiceResponse> GetAsync(int id);
        Task<ServiceResponse> CreateAsync(Guid id, int kitId, String url);
        Task<ServiceResponse> RemoveAsync(int kitId);
        Task<ServiceResponse> UpdateAsync(Guid id, int kitId, string url);
    }
}
