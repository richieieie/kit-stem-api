using kit_stem_api.Models.DTO;

namespace kit_stem_api.Services.IServices
{
    public interface ILevelService
    {
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> GetByIdAsync(int id);
        Task<ServiceResponse> CreateAsync(LevelCreateDTO level);
        Task<ServiceResponse> UpdateAsync(LevelUpdateDTO level);
        Task<ServiceResponse> RemoveByIdAsync(int id);
        Task<ServiceResponse> RestoreByIdAsync(int id);

    }
}
