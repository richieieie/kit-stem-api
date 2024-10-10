using KST.Api.Data;
using KST.Api.Models.Domain;

namespace KST.Api.Repositories
{
    public class LevelRepository : GenericRepository<Level>
    {
        public LevelRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }
    }
}
