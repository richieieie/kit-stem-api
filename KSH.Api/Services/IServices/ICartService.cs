using KSH.Api.Models.DTO;
using KSH.Api.Models.DTO.Request;

namespace KSH.Api.Services.IServices
{
    public interface ICartService
    {
        Task<ServiceResponse> GetByIdAsync(string userName);
        Task<ServiceResponse> CreateAsync(string userName, CartDTO cartDTO);
        Task<ServiceResponse> UpdateAsync(string userName, CartDTO cartDTO);
        Task<ServiceResponse> RemoveByPackageIdAsync(string userName, int id);
        Task<ServiceResponse> RemoveAllAsync(string userName);
    }
}
