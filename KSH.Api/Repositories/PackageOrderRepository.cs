using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KSH.Api.Constants;
using KSH.Api.Data;
using KSH.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KSH.Api.Repositories
{
    public class PackageOrderRepository : GenericRepository<PackageOrder>
    {
        public PackageOrderRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<(IEnumerable<PackageOrder>, int)> GetFilterAsync(
            Expression<Func<PackageOrder, bool>>? filter = null,
            Func<IQueryable<PackageOrder>, IOrderedQueryable<PackageOrder>>? orderBy = null,
            int? skip = null,
            int? take = null
        )
        {
            Func<IQueryable<PackageOrder>, IQueryable<PackageOrder>> includeQuery = includeQuery => includeQuery
                                                                                                .Include(p => p.Package).ThenInclude(p => p.PackageLabs).ThenInclude(p => p.Lab);
            var (packageOrders, totalPages) = await base.GetFilterAsync(filter, orderBy, skip, take, includeQuery);
            return (packageOrders, totalPages);
        }

        public async Task<IEnumerable<object>> GetTopPackageOrderedByYear(int top, int year)
        {
            var packageSales = await _dbContext.PackageOrders.Include(po => po.Order).Include(po => po.Package).ThenInclude(p => p.Kit)
                                        .Where(po => po.Order.CreatedAt.Year == year && po.Order.ShippingStatus == OrderFulfillmentConstants.OrderSuccessStatus)
                                        .GroupBy(po => new
                                        {
                                            PackageId = po.Package.Id,
                                            PackageName = po.Package.Name,
                                            KitId = po.Package.Kit.Id,
                                            KitName = po.Package.Kit.Name
                                        })
                                        .Select(g => new
                                        {
                                            g.Key.PackageId,
                                            g.Key.PackageName,
                                            g.Key.KitId,
                                            g.Key.KitName,
                                            SoldQuantity = g.Sum(po => po.PackageQuantity)
                                        })
                                        .OrderByDescending(ps => ps.SoldQuantity)
                                        .Take(top)
                                        .ToListAsync<object>();
            return packageSales;
        }
    }
}