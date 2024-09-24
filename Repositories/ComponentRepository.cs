using kit_stem_api.Data;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using kit_stem_api.Repositories;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using kit_stem_api.Models.Domain;

namespace kit_stem_api.Repositories
{
    public class ComponentRepository : GenericRepository<Models.Domain.Component>
    {
        

        public ComponentRepository(KitStemDbContext dbContext) : base(dbContext) { }
        
    }
}
