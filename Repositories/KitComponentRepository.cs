using kit_stem_api.Data;
using kit_stem_api.Models.Domain;

namespace kit_stem_api.Repositories
{
    public class KitComponentRepository : GenericRepository<KitComponent>
    {
        public KitComponentRepository(KitStemDbContext context) : base(context) { }
    }
}
