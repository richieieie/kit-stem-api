using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using KSH.Api.Constants;
using KSH.Api.Data;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Models.DTO.Response;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;

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
        public async Task<List<PackageOrderDTO>> GetPackageOrder(List<Guid> listOrderId)
        {
            var PackageOrderDTO = await _dbContext.PackageOrders
            .Where(p => listOrderId.Contains(p.OrderId))
            .GroupBy(p => p.PackageId)
            .Select(g => new PackageOrderDTO
            {
                PackageId = g.Key,
                PackageQuantity = g.Sum(p => p.PackageQuantity),
            })
            .ToListAsync();
            return PackageOrderDTO;
        }
        public async Task<(IEnumerable<PackageTopSaleDTO>, IEnumerable<PackageTopSaleLabPriceDTO>)> GetTopPackageSale(TopPackageSaleGetDTO packageSaleGetDTO)
        {
            var PackageTopSaleQuery = _dbContext.PackageOrders
                                        .Where(po => po.Order.ShippingStatus == packageSaleGetDTO.ShippingStatus)
                                        .Where(po => po.Order.DeliveredAt >= packageSaleGetDTO.FromDate && po.Order.DeliveredAt <= packageSaleGetDTO.ToDate)
                                        .GroupBy(po => new
                                        {
                                            po.PackageId,
                                            PackageName = po.Package.Name,
                                            KitID = po.Package.KitId,
                                            KitName = po.Package.Kit.Name
                                        })
                                            .Select(g => new PackageTopSaleDTO
                                            {
                                                PackageId = g.Key.PackageId,
                                                PackageName = g.Key.PackageName,
                                                TotalPackagePrice = g.Sum(po => po.Package.Price * po.PackageQuantity),
                                                TotalProfit = g.Sum(po => po.Package.Price * po.PackageQuantity) - g.Sum(po => po.Package.Kit.PurchaseCost * po.PackageQuantity),
                                                KitId = g.Key.KitID,
                                                KitName = g.Key.KitName
                                            });
                                        
            List<PackageTopSaleDTO>? packageSaleresults = null;
            if (packageSaleGetDTO.BySale)
            {
                packageSaleresults = await PackageTopSaleQuery
                                    .OrderByDescending(p => p.TotalPackagePrice)
                                    .Take(packageSaleGetDTO.PackageTop)
                                    .ToListAsync();
            }
            else
            {
                packageSaleresults = await PackageTopSaleQuery
                                    .OrderByDescending(p => p.TotalProfit)
                                    .Take(packageSaleGetDTO.PackageTop)
                                    .ToListAsync();
            }
            var topPackageId = packageSaleresults
                                    .OrderByDescending(p => p.TotalPackagePrice)
                                    .Select(p => p.PackageId)
                                    .ToList();
            var LabSaleresults = await _dbContext.PackageOrders
                                            .Join(_dbContext.Packages, po => po.PackageId, p => p.Id, (po, p) => new { po, p })
                                            .Join(_dbContext.UserOrders, po_p => po_p.po.OrderId, o => o.Id, (po_p, o) => new { po_p.po, po_p.p, o })
                                            .GroupJoin(_dbContext.PackageLabs, po_p_o => po_p_o.p.Id, pl => pl.PackageId, (po_p_o, pls) => new { po_p_o.po, po_p_o.p, po_p_o.o, pls })
                                            .SelectMany(x => x.pls.DefaultIfEmpty(), (x, pl) => new { x.po, x.p, x.o, pl })
                                            .GroupJoin(_dbContext.Labs, x => x.pl.LabId, l => l.Id, (x, labs) => new { x.po, x.p, x.o, labs })
                                            .SelectMany(x => x.labs.DefaultIfEmpty(), (x, l) => new { x.po, x.p, x.o, l })
                                            .Where(x => x.o.ShippingStatus == OrderFulfillmentConstants.OrderSuccessStatus)
                                            .Where(x => x.o.DeliveredAt >= packageSaleGetDTO.FromDate && x.o.DeliveredAt <= packageSaleGetDTO.ToDate)
                                            .Where(x => topPackageId.Contains(x.po.PackageId))
                                            .GroupBy(x => x.po.PackageId)
                                            .Select(g => new PackageTopSaleLabPriceDTO
                                            {
                                                PackageId = g.Key,
                                                TotalLabPrice = g.Sum(x => (x.l != null ? x.l.Price : 0) * x.po.PackageQuantity)
                                            })
                                            .OrderBy(x => x.PackageId)
                                            .ToListAsync();
            return (packageSaleresults, LabSaleresults);
        }
        #region method help for PackageSale
        #endregion
    }
}