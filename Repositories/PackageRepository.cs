using System.Linq.Expressions;
using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Repositories
{
    public class PackageRepository : GenericRepository<Package>
    {
        public PackageRepository(KitStemDbContext context) : base(context)
        {
        }
        public new async Task<Package?> GetByIdAsync(int id)
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
            var (packages, totalPages) = await GetFilterAsync(
                                                                filter, orderBy, skip, take,
                                                                query
                                                            );

            return (packages, totalPages);
        }
    }
}
