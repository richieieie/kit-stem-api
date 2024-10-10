using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KST.Api.Data;
using KST.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KST.Api.Repositories
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