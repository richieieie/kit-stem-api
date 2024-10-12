using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO;

namespace KSH.Api.Services.IServices
{
    public interface ICategoryService
    {
        Task<ServiceResponse> GetAsync();
        Task<ServiceResponse> GetByIdAsync(int id);
        Task<ServiceResponse> CreateAsync(CategoryCreateDTO category);
        Task<ServiceResponse> UpdateAsync(CategoryUpdateDTO category);
        Task<ServiceResponse> RemoveByIdAsync(int id);
        Task<ServiceResponse> RestoreByIdAsync(int id);
    }
}
