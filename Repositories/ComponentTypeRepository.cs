using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Repositories
{
    public class ComponentTypeRepository : IComponentTypeRepository
    {
        private readonly KitStemDbContext _dbContext;

        public ComponentTypeRepository(KitStemDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ComponentsType> CreateComponentTypeAsync(ComponentsType componentType)
        {
            await _dbContext.ComponentsTypes.AddAsync(componentType);
            await _dbContext.SaveChangesAsync();
            return componentType;
        }

        public async Task<ComponentsType> DeleteComponentTypeAsync(int Id)
        {
            var deleteComponentType = await _dbContext.ComponentsTypes.FindAsync(Id);
            _dbContext.ComponentsTypes.Remove(deleteComponentType);
            await _dbContext.SaveChangesAsync();
            return deleteComponentType;
        }

        public async Task<List<ComponentTypeDTO>> GetComponentTypesAsync()
        {
            var componentTypes = await _dbContext.ComponentsTypes.ToListAsync();
            return componentTypes.Select(component => new ComponentTypeDTO
            {
                Id = component.Id,
                Name = component.Name
            }).ToList();
        }

        public async Task<ComponentsType> UpdateComponentTypeAsync(int Id, ComponentTypeUpdateDTO componentsType)
        {
            var updateComponentType = await _dbContext.ComponentsTypes.FindAsync(Id);

            updateComponentType.Name = componentsType.Name;
            await _dbContext.SaveChangesAsync();

            return updateComponentType;
        }
    }
}
