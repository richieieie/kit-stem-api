using System.Linq.Expressions;
using KST.Api.Data;
using KST.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KST.Api.Repositories
{
    public class OrderRepository : GenericRepository<UserOrders>
    {
        public OrderRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<(IEnumerable<UserOrders>, int)> GetFilterAsync(
            Expression<Func<UserOrders, bool>>? filter = null,
            Func<IQueryable<UserOrders>, IOrderedQueryable<UserOrders>>? orderBy = null,
            int? skip = null,
            int? take = null
        )
        {
            Func<IQueryable<UserOrders>, IQueryable<UserOrders>> includeQuery = includeQuery => includeQuery
                                                                                                .Include(p => p.User);
            var (orders, totalPages) = await base.GetFilterAsync(filter, orderBy, skip, take, includeQuery);
            return (orders, totalPages);
        }

        public override async Task<UserOrders?> GetByIdAsync(Guid id)
        {
            return await _dbContext.UserOrders
                            .Include(p => p.PackageOrders)
                                .ThenInclude(p => p.Package)
                                    .ThenInclude(p => p.Kit)
                            .Include(o => o.PackageOrders)
                                .ThenInclude(po => po.Package)
                                    .ThenInclude(p => p.Level)
                            .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}