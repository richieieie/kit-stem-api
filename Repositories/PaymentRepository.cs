using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<Payment?> GetByOrderId(Guid id)
        {
            return await _dbContext.Payments.FirstOrDefaultAsync(p => p.OrderId == id); ;
        }
    }
}