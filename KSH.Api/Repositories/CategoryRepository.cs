using KST.Api.Data;
using KST.Api.Models.Domain;
using KST.Api.Models.DTO;
using KST.Api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using KST.Api.Repositories;


namespace KST.Api.Repositories
{
    public class CategoryRepository : GenericRepository<KitsCategory>
    {


        public CategoryRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }


    }
}
