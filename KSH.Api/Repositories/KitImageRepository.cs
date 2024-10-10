using KST.Api.Data;
using KST.Api.Models.Domain;

namespace KST.Api.Repositories
{
    public class KitImageRepository : GenericRepository<KitImage>
    {
        public KitImageRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }
    }
}
