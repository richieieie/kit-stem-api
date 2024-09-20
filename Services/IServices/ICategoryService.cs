using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;

namespace kit_stem_api.Services.IServices
{
    public interface ICategoryService
    {
        Task<ServiceResponse> GetCategoriesAsync();
        Task<ServiceResponse> AddCategoriesAsync(CategoryCreateDTO categoryCreateDTO);
        Task<ServiceResponse> UpdateCategoriesAsync(int Id, CategoryUpdateDTO categoryUpdateDTO);
        Task<ServiceResponse> DeleteCategoriesAsync(int Id);
    }
}
