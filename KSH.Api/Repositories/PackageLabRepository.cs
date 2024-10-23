using KSH.Api.Data;
using KSH.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KSH.Api.Repositories
{
    public class PackageLabRepository : GenericRepository<PackageLab>
    {
        public PackageLabRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<List<Guid>> GetByPackageId(int packageId)
        {
            var packageLabs = await _dbContext.PackageLabs
                .Where(pl => pl.PackageId == packageId)
                .Select(pl => pl.LabId)
                .ToListAsync();
            return packageLabs;
        }
    }
}