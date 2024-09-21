using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;

namespace kit_stem_api.Repositories.IRepositories
{
    public interface IComponentTypeRepository
    {
        Task<List<ComponentTypeDTO>> GetComponentTypesAsync();
        Task<ComponentsType> CreateComponentTypeAsync(ComponentsType componentType);
        Task<ComponentsType> UpdateComponentTypeAsync(int Id, ComponentTypeUpdateDTO componentsType);
        Task<ComponentsType> DeleteComponentTypeAsync(int Id);
    }
}
