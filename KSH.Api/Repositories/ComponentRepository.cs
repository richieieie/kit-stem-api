using KSH.Api.Data;
using KSH.Api.Models.DTO;
using KSH.Api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using KSH.Api.Repositories;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using KSH.Api.Models.Domain;

namespace KSH.Api.Repositories
{
    public class ComponentRepository : GenericRepository<Models.Domain.Component>
    {


        public ComponentRepository(KitStemDbContext dbContext) : base(dbContext) { }

    }
}
