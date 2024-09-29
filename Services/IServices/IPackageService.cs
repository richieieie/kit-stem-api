using kit_stem_api.Models.DTO;
using kit_stem_api.Models.DTO.Request;

namespace kit_stem_api.Services.IServices
{
    public interface IPackageService
    {
        Task<(ServiceResponse, int)> CreateAsync(PackageCreateDTO packageCreateDTO);
        Task<ServiceResponse> GetAsync(PackageGetFilterDTO packageGetFilterDTO);
        Task<ServiceResponse> GetByIdAsync(int id);
        Task<ServiceResponse> UpdateAsync(PackageUpdateDTO packageUpdateDTO);
    }
}
