using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;

namespace kit_stem_api.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        Task<List<CategoryDTO>> GetCategoriesAsync();

        Task<KitsCategory> AddCategoryAsync(KitsCategory kitsCategory);

        Task<KitsCategory> UpdateCategoryAsync(int Id, CategoryUpdateDTO categoryUpdateDTO);
        Task<KitsCategory> DeleteCategoryAsync(int Id);

    }
}
