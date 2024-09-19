using kit_stem_api.Models.DTO;

namespace kit_stem_api.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        Task<List<CategoryDTO>> GetCategoriesAsync();

        Task<bool> AddCategoryAsync(CategoryDTO categoryDTO);

        Task<bool> UpdateCategoryAsync(int Id, CategoryDTO categoryDTO);
        Task<bool> DeleteCategoryAsync(int Id);

    }
}
