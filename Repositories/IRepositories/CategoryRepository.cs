using kit_stem_api.Data;
using kit_stem_api.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Repositories.IRepositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly KitStemDbContext _dbContext;

        public CategoryRepository(KitStemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<bool> AddCategoryAsync(CategoryDTO categoryDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCategoryAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CategoryDTO>> GetCategoriesAsync()
        {
            var categories = await _dbContext.KitsCategories.ToListAsync();
            return categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            }).ToList();
        }

        public Task<bool> UpdateCategoryAsync(int Id, CategoryDTO categoryDTO)
        {
            throw new NotImplementedException();
        }
    }
}
