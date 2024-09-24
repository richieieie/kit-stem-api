using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;

namespace kit_stem_api.Repositories.IRepositories
{
    public interface ICategoryRepository
    {
        Task<List<KitsCategory>> GetAsync();

        Task<bool> AddAsync(KitsCategory kitsCategory);

        Task<bool> UpdateAsync(KitsCategory kitsCategory);
        Task<bool> DeleteAsync(KitsCategory kitsCategory);

    }
}
