using KSH.Api.Data;
using KSH.Api.Models.Domain;

namespace KSH.Api.Repositories
{
    public class KitImageRepository : GenericRepository<KitImage>
    {
        public KitImageRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }
    }
}
