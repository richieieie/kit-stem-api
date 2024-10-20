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

        public async Task<Guid> GetByPackageId(int packageId)
        {
            var packageLab = await _dbContext.PackageLabs.FirstOrDefaultAsync(pl => pl.PackageId == packageId);
            return packageLab!.LabId;
        }
    }
}