using KSH.Api.Models.DTO;

namespace KSH.Api.Services.IServices
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
