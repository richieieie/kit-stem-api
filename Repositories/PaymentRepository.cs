using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kit_stem_api.Data;
using kit_stem_api.Models.Domain;

namespace kit_stem_api.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>
    {
        public PaymentRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }
    }
}