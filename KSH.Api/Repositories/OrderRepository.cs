using System.Linq.Expressions;
using KSH.Api.Data;
using KSH.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KSH.Api.Repositories
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
                                                                                                .Include(o => o.User);
            var (orders, totalPages) = await base.GetFilterAsync(filter, orderBy, skip, take, includeQuery);
            return (orders, totalPages);
        }

        public override async Task<UserOrders?> GetByIdAsync(Guid id)
        {
            return await _dbContext.UserOrders
                            .Include(p => p.PackageOrders)
                                .ThenInclude(p => p.Package)
                                    .ThenInclude(p => p.Kit)
                                        .ThenInclude(k => k.KitImages)
                            .Include(p => p.PackageOrders)
                                .ThenInclude(p => p.Package)
                                    .ThenInclude(p => p.PackageLabs)
                            .Include(o => o.PackageOrders)
                                .ThenInclude(po => po.Package)
                                    .ThenInclude(p => p.Level)
                            .Include(o => o.OrderSupports)
                                .ThenInclude(os => os.Lab)
                            .Include(o => o.OrderSupports)
                                .ThenInclude(os => os.Package)
                            .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<int> CountTotalOrders(DateTimeOffset? fromDate, DateTimeOffset? toDate, string? shippingStatus)
        {
            throw new NotImplementedException();
        }
    }
}