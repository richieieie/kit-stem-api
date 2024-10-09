using kit_stem_api.Data;
using kit_stem_api.Models.Domain;

namespace kit_stem_api.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>
    {
        public UserRepository(KitStemDbContext context) : base(context)
        {
        }
    }
}