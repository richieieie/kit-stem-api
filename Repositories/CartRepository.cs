using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Repositories
{
    public class CartRepository : GenericRepository<Cart>
    {
        public CartRepository(KitStemDbContext context) : base(context)
        {
        }

        //public new async Task<List<Package>> GetByIdAsync(string userId)
        //{
        //    return await _dbSet
        //        .Where(c => c.UserId == userId)
        //        .Include(c => c.Package)
        //        .Select(c => c.Package)
        //        .ToListAsync();
        //}
    }
}
