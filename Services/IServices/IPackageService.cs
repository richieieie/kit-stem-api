using kit_stem_api.Models.DTO;

namespace kit_stem_api.Services.IServices
{
    public interface IPackageService
    {
        Task<ServiceResponse> GetAsync(PackageGetDTO packageGetDTO);
    }
}
