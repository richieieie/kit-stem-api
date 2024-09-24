using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using kit_stem_api.Repositories;

namespace kit_stem_api.Repositories
{
    public class ComponentTypeRepository : GenericRepository<ComponentsType> 
    {
        public ComponentTypeRepository(KitStemDbContext dbContext) : base(dbContext) { }
    }
}
