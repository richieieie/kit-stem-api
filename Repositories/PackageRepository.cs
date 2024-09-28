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
                            .Include(p => p.Kit)
                            .Include(p => p.PackageLabs)
                            .ThenInclude(pl => pl.Lab)
                            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<(IEnumerable<Package>, int)> GetFilterAsync(
            Expression<Func<Package, bool>>? filter = null,
            Func<IQueryable<Package>, IOrderedQueryable<Package>>? orderBy = null,
            int? skip = null,
            int? take = null
        )
        {
            var (packages, totalPages) = await GetFilterAsync(
                                                                filter, orderBy, skip, take,
                                                                query => query.Include(p => p.Kit),
                                                                query => query.Include(p => p.Level),
                                                                query => query.Include(p => p.Kit.Category),
                                                                query => query.Include(p => p.PackageLabs).ThenInclude(pl => pl.Lab)
                                                            );

            return (packages, totalPages);
        }
    }
}
