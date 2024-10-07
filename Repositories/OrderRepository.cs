using System.Linq.Expressions;
using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Repositories
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
                                                                                                .Include(o => o.Payment)
                                                                                                .ThenInclude(p => p.Method)
                                                                                                .Include(p => p.User);
            var (orders, totalPages) = await GetFilterAsync(filter, orderBy, skip, take, includeQuery);
            return (orders, totalPages);
        }

        public new async Task<UserOrders?> GetByIdAsync(Guid id)
        {
            return await _dbContext.UserOrders.Include(o => o.Payment)
                                                .ThenInclude(p => p.Method)
                                                .Include(p => p.PackageOrders)
                                                .FirstOrDefaultAsync(o => o.Id == id);
        }
    }
}