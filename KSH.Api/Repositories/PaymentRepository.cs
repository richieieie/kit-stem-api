using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KSH.Api.Data;
using KSH.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KSH.Api.Repositories
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