using kit_stem_api.Data;
using kit_stem_api.Repositories;

namespace PMS.Repository
{
    public class UnitOfWork
    {
        private readonly KitStemDbContext _dbContext;
        public LabRepository LabRepository { get; }

        public UnitOfWork(KitStemDbContext dbContext)
        {
            _dbContext = dbContext;
            LabRepository = new LabRepository(_dbContext);
        }

    }
}
