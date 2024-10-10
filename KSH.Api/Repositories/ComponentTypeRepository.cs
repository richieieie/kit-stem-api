using KST.Api.Data;
using KST.Api.Models.Domain;
using KST.Api.Models.DTO;
using KST.Api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using KST.Api.Repositories;

namespace KST.Api.Repositories
{
    public class ComponentTypeRepository : GenericRepository<ComponentsType>
    {
        public ComponentTypeRepository(KitStemDbContext dbContext) : base(dbContext) { }
    }
}
