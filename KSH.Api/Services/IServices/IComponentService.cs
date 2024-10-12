using KSH.Api.Models.DTO;

namespace KSH.Api.Services.IServices
{
    public interface IComponentService
    {
        Task<ServiceResponse> GetAllAsync();
        Task<ServiceResponse> GetByIdAsync(int id);
        Task<ServiceResponse> CreateAsync(ComponentCreateDTO component);
        Task<ServiceResponse> UpdateAsync(ComponentUpdateDTO component);
        Task<ServiceResponse> RemoveByIdAsync(int id);
        Task<ServiceResponse> RestoreByIdAsync(int id);

    }
}
