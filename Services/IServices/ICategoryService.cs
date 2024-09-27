using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;

namespace kit_stem_api.Services.IServices
{
    public interface ICategoryService
    {
        Task<ServiceResponse> GetAsync();
        Task<ServiceResponse> GetByIdAsync(int id);
        Task<ServiceResponse> CreateAsync(CategoryCreateDTO category);
        Task<ServiceResponse> UpdateAsync(CategoryUpdateDTO category);
        Task<ServiceResponse> RemoveByIdAsync(int id);
    }
}
