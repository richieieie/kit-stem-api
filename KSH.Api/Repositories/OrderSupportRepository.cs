using KSH.Api.Data;
using KSH.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KSH.Api.Repositories
{
    public class OrderSupportRepository : GenericRepository<OrderSupport>
    {
        public OrderSupportRepository(KitStemDbContext context) : base(context)
        {
        }

        public async Task<OrderSupport?> GetByLabIdAndOrderIdAsync(Guid labId, Guid orderId)
        {
            return await _dbContext.OrderSupports
                                    .Include(os => os.Lab)
                                    .Include(os => os.Order)
                                    .Include(os => os.Package)
                                    .FirstOrDefaultAsync(os => os.OrderId == orderId && os.LabId == labId);
        }
    }
}