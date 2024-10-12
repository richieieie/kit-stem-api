using KSH.Api.Data;
using KSH.Api.Models.Domain;

namespace KSH.Api.Repositories
{
    public class LevelRepository : GenericRepository<Level>
    {
        public LevelRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }
    }
}
