using kit_stem_api.Data;
using kit_stem_api.Models.Domain;

namespace kit_stem_api.Repositories
{
    public class LevelRepository : GenericRepository<Level>
    {
        public LevelRepository(KitStemDbContext dbContext) : base(dbContext)
        {
            
        }
    }
}
