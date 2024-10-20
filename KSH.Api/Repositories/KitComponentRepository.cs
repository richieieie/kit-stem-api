using KSH.Api.Data;
using KSH.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KSH.Api.Repositories
{
    public class KitComponentRepository : GenericRepository<KitComponent>
    {
        public KitComponentRepository(KitStemDbContext dbContext) : base(dbContext)
        {
            
        }
        public async Task<bool> DeleteAsync(int kitId)
        {
            var kitComponent = await _dbContext.KitComponents
                                               .Where(kc => kc.KitId == kitId)
                                               .ToListAsync();
            if(kitComponent.Any())
            {
                _dbContext.KitComponents.RemoveRange(kitComponent);
            }
            else
            {
                return true;
            }
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
