using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;


namespace kit_stem_api.Repositories.IRepositories
{
    public interface IComponentRepository
    {
        Task<List<ComponentDTO>> GetComponentsAsync();
        Task<Component> CreateComponentAsync(Component component);
        Task<Component> UpdateComponentAsync(int Id, ComponentUpdateDTO component);
        Task<Component> DeleteComponentAsync(int Id);

    }
}
