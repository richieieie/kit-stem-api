﻿using KSH.Api.Data;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Request;
using Microsoft.EntityFrameworkCore;

namespace KSH.Api.Repositories
{
    public class KitRepository : GenericRepository<Kit>
    {
        public KitRepository(KitStemDbContext dbContext) : base(dbContext)
        {

        }
        public async Task<int> CreateReturnIdAsync(Kit entity)
        {
            _dbContext.Add(entity);
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return entity.Id;
            }
            else
            {
                return 0;
            }
        }

        public async Task<int> GetMaxIdAsync()
        {
            var maxId = await _dbContext.Kits.MaxAsync(k => k.Id);
            return maxId;
        }

        public async Task<long> GetPurchaseCostById(int kitId)
        {
            var kit = await _dbContext.Kits.FirstOrDefaultAsync(k => k.Id == kitId);
            return kit!.PurchaseCost;
        }
    }
}
