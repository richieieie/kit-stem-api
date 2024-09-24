using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Repositories.IRepositories;
using PMS.Repository.Base;

namespace kit_stem_api.Repositories
{
    public class LabRepository : GenericRepository<Lab>
    {
        public LabRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }
    }
}