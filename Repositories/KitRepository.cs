using kit_stem_api.Data;
using kit_stem_api.Models.Domain;

namespace kit_stem_api.Repositories
{
    public class KitRepository : GenericRepository<Kit>
    {
        public KitRepository(KitStemDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
