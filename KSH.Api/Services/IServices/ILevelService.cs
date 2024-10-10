using KST.Api.Models.DTO;

namespace KST.Api.Services.IServices
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
