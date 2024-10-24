using KSH.Api.Data;
using KSH.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KSH.Api.Repositories
{
    public class ShippingFeeRepository : GenericRepository<ShippingFee>
    {
        public ShippingFeeRepository(KitStemDbContext context) : base(context)
        {
        }

        public async Task<ShippingFee> GetShippingFee(double? distance)
        {
            var shippingFee = await _dbContext.ShippingFees
                .Where(sf => sf.FromDistance <= distance && sf.ToDistance >= distance)
                .FirstOrDefaultAsync();
            return shippingFee!;
        }
    }
}
