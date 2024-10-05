using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Repositories
{
    public class KitRepository : GenericRepository<Kit>
    {
        public KitRepository(KitStemDbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<int> GetMaxIdAsync()
        {
            var maxId = await _dbContext.Kits.MaxAsync(k => k.Id);
            return maxId;
        }
    }
}
