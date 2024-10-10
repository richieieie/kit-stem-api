using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KST.Api.Data;
using KST.Api.Models.Domain;

namespace KST.Api.Repositories
{
    public class PackageLabRepository : GenericRepository<PackageLab>
    {
        public PackageLabRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }
    }
}