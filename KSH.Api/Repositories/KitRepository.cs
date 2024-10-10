using KST.Api.Data;
using KST.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KST.Api.Repositories
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
