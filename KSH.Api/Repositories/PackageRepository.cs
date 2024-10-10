using System.Linq.Expressions;
using KST.Api.Data;
using KST.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KST.Api.Repositories
{
    public class PackageRepository : GenericRepository<Package>
    {
        public PackageRepository(KitStemDbContext context) : base(context)
        {
        }
        public override async Task<Package?> GetByIdAsync(int id)
        {
            return await _dbContext.Packages
                            .Include(p => p.PackageLabs)
                            .ThenInclude(pl => pl.Lab)
                            .ThenInclude(pll => pll.Level)
                            .Include(p => p.Level)
                            .Include(p => p.Kit)
                            .ThenInclude(k => k.Category)
                            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<(IEnumerable<Package>, int)> GetFilterAsync(
            Expression<Func<Package, bool>>? filter = null,
            Func<IQueryable<Package>, IOrderedQueryable<Package>>? orderBy = null,
            int? skip = null,
            int? take = null,
            bool includeLabs = false
        )
        {
            Func<IQueryable<Package>, IQueryable<Package>> query = query => query.Include(p => p.Level).Include(p => p.Kit).ThenInclude(k => k.Category);
            if (includeLabs)
            {
                query = query => query.Include(p => p.PackageLabs).ThenInclude(pl => pl.Lab).ThenInclude(pll => pll.Level).Include(p => p.Level).Include(p => p.Kit).ThenInclude(k => k.Category);
            }
            var (packages, totalPages) = await base.GetFilterAsync(
                                                                filter, orderBy, skip, take,
                                                                query
                                                            );

            return (packages, totalPages);
        }
    }
}
