using KST.Api.Models.DTO;

namespace KST.Api.Services.IServices
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
