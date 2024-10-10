using KST.Api.Data;
using KST.Api.Models.DTO;
using KST.Api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using KST.Api.Repositories;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using KST.Api.Models.Domain;

namespace KST.Api.Repositories
{
    public class ComponentRepository : GenericRepository<Models.Domain.Component>
    {


        public ComponentRepository(KitStemDbContext dbContext) : base(dbContext) { }

    }
}
