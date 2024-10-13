using KSH.Api.Data;
using KSH.Api.Models.Domain;
using KSH.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KST.Api.Repositories
{
    public class LabSupportRepository : GenericRepository<LabSupport>
    {
        public LabSupportRepository(KitStemDbContext context) : base(context) { }

        public async Task<LabSupport> GetByOrderSupportId(Guid orderSupportId)
        {
            var labSupport = await _dbContext.LabSupports.Where(ls =>
            ls.OrderSupportId == orderSupportId &&
            ls.IsFinished == false).FirstOrDefaultAsync();
            return labSupport!;

        }
    }
}
