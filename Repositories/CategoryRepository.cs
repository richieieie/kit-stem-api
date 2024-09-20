using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly KitStemDbContext _dbContext;

        public CategoryRepository(KitStemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<KitsCategory> AddCategoryAsync(KitsCategory kitsCategory)
        {
            await _dbContext.KitsCategories.AddAsync(kitsCategory);
            await _dbContext.SaveChangesAsync();
            return kitsCategory;
        }

        public async Task<KitsCategory> DeleteCategoryAsync(int Id)
        {
            var category = await _dbContext.KitsCategories.FindAsync(Id);
            _dbContext.KitsCategories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return category;
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

        public async Task<KitsCategory> UpdateCategoryAsync(int Id, CategoryUpdateDTO categoryUpdateDTO)
        {
            var category = await _dbContext.KitsCategories.FindAsync(Id);
            
            category.Name = categoryUpdateDTO.Name;
            category.Description = categoryUpdateDTO.Description;

            await _dbContext.SaveChangesAsync();
            return category;
        }
    }
}
