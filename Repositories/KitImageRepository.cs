using kit_stem_api.Data;
using kit_stem_api.Models.Domain;

namespace kit_stem_api.Repositories
{
    public class KitImageRepository : GenericRepository<KitImage>
    {
        public KitImageRepository(KitStemDbContext dbContext) : base(dbContext) 
        {
            
        }
    }
}
