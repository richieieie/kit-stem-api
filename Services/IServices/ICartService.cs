using kit_stem_api.Models.DTO;
using kit_stem_api.Models.DTO.Request;

namespace kit_stem_api.Services.IServices
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
