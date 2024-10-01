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
            Func<IQueryable<UserOrders>, IQueryable<UserOrders>> query = query => query.Include(o => o.Payment);
            var (orders, totalPages) = await GetFilterAsync(filter, orderBy, skip, take, query);
            return (orders, totalPages);
        }
    }
}