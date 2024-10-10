using KST.Api.Models.Domain;
using KST.Api.Models.DTO.Request;

namespace KST.Api.Services.IServices
{
    public interface IKitImageService
    {
        Task<ServiceResponse> GetAsync(int id);
        Task<ServiceResponse> CreateAsync(Guid id, int kitId, String url);
        Task<ServiceResponse> RemoveAsync(int kitId);
        Task<ServiceResponse> UpdateAsync(Guid id, int kitId, string url);
    }
}
