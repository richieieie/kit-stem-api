using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Request;

namespace KSH.Api.Services.IServices
{
    public interface IKitImageService
    {
        Task<ServiceResponse> GetAsync(int id);
        Task<ServiceResponse> CreateAsync(Guid id, int kitId, String url);
        Task<ServiceResponse> RemoveAsync(int kitId);
        Task<ServiceResponse> UpdateAsync(Guid id, int kitId, string url);
    }
}
