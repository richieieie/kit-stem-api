using System.Linq.Expressions;
using KSH.Api.Data;
using KSH.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace KSH.Api.Repositories
{
    public class LabRepository : GenericRepository<Lab>
    {
        public LabRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }
        public override async Task<Lab?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Labs
                                    .Include(l => l.Kit)
                                    .ThenInclude(lk => lk.Category)
                                    .Include(l => l.Level)
                                    .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<(IEnumerable<Lab>, int)> GetFilterAsync(
            Expression<Func<Lab, bool>>? filter = null,
            Func<IQueryable<Lab>, IOrderedQueryable<Lab>>? orderBy = null,
            int? skip = null,
            int? take = null
        )
        {
            var (labs, totalPages) = await base.GetFilterAsync(filter, orderBy, skip, take,
                                                            query => query.Include(l => l.Kit),
                                                            query => query.Include(l => l.Level),
                                                            query => query.Include(l => l.Kit.Category)
                                                        );
            return (labs, totalPages);
        }
        public async Task<(IEnumerable<Lab>, int)> GetByKitIdAsync(int kitId)
        {
            var (labs, totalPages) = await GetFilterAsync(l => l.KitId == kitId);

            return (labs, totalPages);
        }

        public async Task<long> GetPurchaseCostById(Guid id)
        {
            var lab = await _dbContext.Labs.FirstOrDefaultAsync(l => l.Id == id);
            return lab!.Price;
        }
    }
}