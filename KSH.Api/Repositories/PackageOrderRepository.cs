using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Threading.Tasks;
using KSH.Api.Data;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Request;
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
    }
}