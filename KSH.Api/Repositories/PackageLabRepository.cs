using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KSH.Api.Data;
using KSH.Api.Models.Domain;

namespace KSH.Api.Repositories
{
    public class PackageLabRepository : GenericRepository<PackageLab>
    {
        public PackageLabRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }
    }
}