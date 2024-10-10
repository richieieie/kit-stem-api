using KST.Api.Models.Domain;
using KST.Api.Models.DTO;

namespace KST.Api.Services.IServices
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
