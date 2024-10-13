using KSH.Api.Data;
using KSH.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<OrderSupport> GetFilterByIdAsync(Guid orderId, int packageId, Guid labId)
        {
            var orderSupport = await _dbContext.OrderSupports.Where(os =>
                os.OrderId == orderId &&
                os.PackageId == packageId && 
                os.LabId == labId).FirstOrDefaultAsync();

            return orderSupport!;

        }
    }
}