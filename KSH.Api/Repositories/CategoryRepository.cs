using KSH.Api.Data;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO;
using KSH.Api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using KSH.Api.Repositories;


namespace KSH.Api.Repositories
{
    public class CategoryRepository : GenericRepository<KitsCategory>
    {


        public CategoryRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }


    }
}
