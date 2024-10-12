using KSH.Api.Data;
using KSH.Api.Models.Domain;

namespace KSH.Api.Repositories
{
    public class UserRepository : GenericRepository<ApplicationUser>
    {
        public UserRepository(KitStemDbContext context) : base(context)
        {
        }
    }
}