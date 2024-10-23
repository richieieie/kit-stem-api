using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KSH.Api.Constants;
using KSH.Api.Data;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Models.DTO.Response;
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
        #region method for AnalyticService
        public async Task<(IEnumerable<PackageTopSaleDTO>, int)> GetTopPackageSale(TopPackageSaleGetDTO packageSaleGetDTO, int sizePerPage)
        {
            var baseQuery = _dbContext.PackageOrders
                                         .Where(po => po.Order.ShippingStatus == packageSaleGetDTO.ShippingStatus)
                                         .Where(po => po.Order.DeliveredAt >= packageSaleGetDTO.FromDate && po.Order.DeliveredAt <= packageSaleGetDTO.ToDate)
                                         .GroupBy(po => new
                                         {
                                             po.PackageId,
                                             KitID = po.Package.KitId,
                                             KitName = po.Package.Kit.Name
                                         })
                                         .Select(g => new PackageTopSaleDTO
                                         {
                                             PackageId = g.Key.PackageId,
                                             TotalPackagePrice = g.Sum(po => po.Package.Price),
                                             KitId = g.Key.KitID,
                                             KitName = g.Key.KitName
                                         });
            int totalRecords = await baseQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalRecords / sizePerPage);
            var results = await baseQuery
                                    .OrderByDescending(p => p.TotalPackagePrice)
                                    .Skip(sizePerPage * packageSaleGetDTO.Page)
                                    .Take(sizePerPage)
                                    .ToListAsync();
            return (results, totalPages);
        }
        #endregion
    }
}