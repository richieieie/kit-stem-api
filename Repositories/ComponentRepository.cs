using kit_stem_api.Data;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace kit_stem_api.Repositories
{
    public class ComponentRepository : IComponentRepository
    {
        private readonly KitStemDbContext _dbContext;

        public ComponentRepository(KitStemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Models.Domain.Component> CreateComponentAsync(Models.Domain.Component component)
        {
            await _dbContext.Components.AddAsync(component);
            await _dbContext.SaveChangesAsync();
            return component;
        }

        public async Task<Models.Domain.Component> DeleteComponentAsync(int Id)
        {
            var component = await _dbContext.Components.FindAsync(Id);
            _dbContext.Components.Remove(component);
            await _dbContext.SaveChangesAsync();
            return component;
        }

        public async Task<List<ComponentDTO>> GetComponentsAsync()
        {
            var components = await _dbContext.Components.ToListAsync();
            return components.Select(c => new ComponentDTO 
            {
                Id = c.Id, 
                TypeId = c.TypeId, 
                Name = c.Name 
            }).ToList();
        }

        public async Task<Models.Domain.Component> UpdateComponentAsync(int Id, ComponentUpdateDTO component)
        {
            var updateComponent = await _dbContext.Components.FindAsync(Id);

            updateComponent.TypeId = component.TypeId;
            updateComponent.Name = component.Name;

            await _dbContext.SaveChangesAsync();
            return updateComponent;
        }
    }
}
