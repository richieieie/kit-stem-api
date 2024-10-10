using KST.Api.Data;
using KST.Api.Models.Domain;

namespace KST.Api.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>
    {
        public UserRepository(KitStemDbContext context) : base(context)
        {
        }
    }
}