using System.Linq.Expressions;
using System.Security.Cryptography;
using KSH.Api.Constants;
using KSH.Api.Data;
using KSH.Api.Models.Domain;
using KSH.Api.Services;
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
                                                                                                .Include(o => o.ShippingFee)
                                                                                                .Include(o => o.User);
            var (orders, totalPages) = await base.GetFilterAsync(filter, orderBy, skip, take, includeQuery);
            return (orders, totalPages);
        }

        public override async Task<UserOrders?> GetByIdAsync(Guid id)
        {
            return await _dbContext.UserOrders
                            .Include(o => o.Payment!.Method)
                            .Include(o => o.ShippingFee)
                            .Include(o => o.PackageOrders)
                                .ThenInclude(po => po.Package)
                                    .ThenInclude(p => p.Kit)
                                        .ThenInclude(k => k.KitImages)
                            .Include(o => o.PackageOrders)
                                .ThenInclude(po => po.Package)
                                    .ThenInclude(p => p.PackageLabs)
                                        .ThenInclude(pl => pl.Lab)
                                            .ThenInclude(l => l.Level)
                            .Include(o => o.PackageOrders)
                                .ThenInclude(po => po.Package)
                                    .ThenInclude(p => p.Level)
                            .Include(o => o.OrderSupports)
                                .ThenInclude(os => os.Lab)
                            .Include(o => o.OrderSupports)
                                .ThenInclude(os => os.Package)
                            .AsSplitQuery()
                            .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<int> CountTotalOrders(DateTimeOffset? fromDate, DateTimeOffset? toDate, string? shippingStatus)
        {
            var count = await _dbContext.UserOrders
                                .Where(o => o.CreatedAt >= fromDate && o.CreatedAt <= toDate && o.ShippingStatus.Contains(shippingStatus ?? ""))
                                .CountAsync();
            return count;
        }

        public async Task<long> SumTotalOrder(DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            return await _dbContext.UserOrders.Where(o =>
            o.DeliveredAt >= fromDate &&
            o.DeliveredAt <= toDate &&
            o.ShippingStatus.Equals(OrderFulfillmentConstants.OrderSuccessStatus)).SumAsync(o => o.TotalPrice);
        }

        public async Task<List<Guid>> GetOrderId(DateTimeOffset? fromDate, DateTimeOffset? toDate)
        {
            return await _dbContext.UserOrders.Where(o =>
            o.DeliveredAt >= fromDate &&
            o.DeliveredAt <= toDate &&
            o.ShippingStatus.Equals(OrderFulfillmentConstants.OrderSuccessStatus)).Select(o => o.Id).ToListAsync();
        }

    }
}