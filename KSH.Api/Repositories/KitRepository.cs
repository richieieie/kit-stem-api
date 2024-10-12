using KSH.Api.Data;
using KSH.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KSH.Api.Repositories
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
