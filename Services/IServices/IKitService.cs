using kit_stem_api.Models.DTO.Request;

namespace kit_stem_api.Services.IServices
{
    public interface IKitService
    {
        Task<ServiceResponse> GetAsync(KitGetDTO kitGetDTO);
        Task<ServiceResponse> GetByIdAsync(int id);
        Task<int> GetMaxIdAsync();
        Task<ServiceResponse> CreateAsync(KitCreateDTO DTO);
        Task<ServiceResponse> UpdateAsync(KitUpdateDTO DTO);
        Task<ServiceResponse> RemoveAsync(int id);
        Task<ServiceResponse> RestoreByIdAsync(int id);
        Task<ServiceResponse> GetPackagesByKitId(int id);
        Task<ServiceResponse> GetLabByKitId(int id);
    }
}
