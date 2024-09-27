using kit_stem_api.Data;
using kit_stem_api.Models.Domain;

namespace kit_stem_api.Repositories
{
    public class PackageRepository : GenericRepository<Package>
    {
        public PackageRepository(KitStemDbContext context) : base(context)
        {
        }
    }
}
