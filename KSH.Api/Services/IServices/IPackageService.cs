using KSH.Api.Models.DTO;
using KSH.Api.Models.DTO.Request;

namespace KSH.Api.Services.IServices
{
    public interface IPackageService
    {
        Task<(ServiceResponse, int)> CreateAsync(PackageCreateDTO packageCreateDTO);
        Task<ServiceResponse> GetAsync(PackageGetFilterDTO packageGetFilterDTO);
        Task<ServiceResponse> GetByIdAsync(int id);
        Task<ServiceResponse> RemoveByIdAsync(int id);
        Task<ServiceResponse> RestoreByIdAsync(int id);
        Task<ServiceResponse> UpdateAsync(PackageUpdateDTO packageUpdateDTO);
    }
}
