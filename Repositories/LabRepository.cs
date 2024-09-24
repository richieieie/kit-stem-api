using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Repositories.IRepositories;

namespace kit_stem_api.Repositories
{
    public class LabRepository : ILabRepository
    {
        private readonly KitStemDbContext _dbContext;
        public LabRepository(KitStemDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> CreateAsync(Lab lab)
        {
            await _dbContext.Labs.AddAsync(lab);
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}