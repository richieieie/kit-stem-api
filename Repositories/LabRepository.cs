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
        public async Task<Lab> CreateAsync(Lab lab)
        {
            await _dbContext.Labs.AddAsync(lab);
            await _dbContext.SaveChangesAsync();

            await _dbContext.Entry(lab).Reference(l => l.Kit).LoadAsync();
            await _dbContext.Entry(lab).Reference(l => l.Level).LoadAsync();
            if (lab.Kit != null)
            {
                await _dbContext.Entry(lab.Kit).Reference(k => k.Category).LoadAsync();  // Load Category for Kit
            }

            return lab;
        }
    }
}