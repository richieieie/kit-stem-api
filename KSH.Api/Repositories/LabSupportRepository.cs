using KSH.Api.Data;
using KSH.Api.Models.Domain;
using KSH.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace KST.Api.Repositories
{
    public class LabSupportRepository : GenericRepository<LabSupport>
    {
        public LabSupportRepository(KitStemDbContext context) : base(context) { }

        public async Task<LabSupport> GetByOrderSupportId(Guid orderSupportId)
        {
            var labSupport = await _dbContext.LabSupports.Where(ls =>
            ls.OrderSupportId == orderSupportId &&
            ls.IsFinished == false).FirstOrDefaultAsync();
            return labSupport!;

        }
        public async Task<(IEnumerable<LabSupport>, int)> GetFilterAsync(
            Expression<Func<LabSupport, bool>>? filter = null,
            Func<IQueryable<LabSupport>, IOrderedQueryable<LabSupport>>? orderBy = null,
            int? skip = null,
            int? take = null
        )
        {
            Func<IQueryable<LabSupport>, IQueryable<LabSupport>> includeQuery = includeQuery => includeQuery
                                                                                                .Include(l => l.OrderSupport.Lab)
                                                                                                .Include(p => p.OrderSupport.Package)
                                                                                                .Include(s => s.Staff)
                                                                                                .Include(c => c.OrderSupport.Order.User);
            var (labSupports, totalPages) = await base.GetFilterAsync(filter, orderBy, skip, take, includeQuery);
            return (labSupports, totalPages);
        }
        public override async Task<LabSupport?> GetByIdAsync(Guid labSupportId)
        {
            return await _dbContext.Set<LabSupport>()
                                        .Include(l => l.OrderSupport.Lab)
                                        .Include(p => p.OrderSupport.Package)
                                        .Include(s => s.Staff)
                                        .Include(c => c.OrderSupport.Order.User)
                                        .FirstOrDefaultAsync(l => l.Id.Equals(labSupportId));
        }
        public async Task<(IEnumerable<LabSupport>, int)> GetByUserIdFilterAsync(
            Expression<Func<LabSupport, bool>>? filter = null,
            Func<IQueryable<LabSupport>, IOrderedQueryable<LabSupport>>? orderBy = null,
            int? skip = null,
            int? take = null
        )
        {
            Func<IQueryable<LabSupport>, IQueryable<LabSupport>> includeQuery = includeQuery => includeQuery
                                                                                                .Include(l => l.OrderSupport.Lab)
                                                                                                .Include(p => p.OrderSupport.Package)
                                                                                                    .ThenInclude(p => p.Kit)
                                                                                                .Include(s => s.Staff)
                                                                                                .Include(c => c.OrderSupport.Order.User);
            var (labSupports, totalPages) = await base.GetFilterAsync(filter, orderBy, skip, take, includeQuery);
            return (labSupports, totalPages);
        }
    }
}
