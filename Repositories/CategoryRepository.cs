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

        public async Task<bool> AddAsync(KitsCategory kitsCategory)
        {
            await _dbContext.KitsCategories.AddAsync(kitsCategory);
            return await _dbContext.SaveChangesAsync() > 0;

        }

        public async Task<bool> DeleteAsync(KitsCategory kitsCategory)
        {
            _dbContext.KitsCategories.Remove(kitsCategory);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<List<KitsCategory>> GetAsync()
        {
            var categories = await _dbContext.KitsCategories.ToListAsync();
            return categories;
        }

        public async Task<bool> UpdateAsync(KitsCategory kitsCategory)
        {
            var tracker = _dbContext.Attach(kitsCategory);
            tracker.State = EntityState.Modified;


            return await _dbContext.SaveChangesAsync() > 0;

        }
    }
}
