using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;

namespace kit_stem_api.Services.IServices
{
    public interface ICategoryService
    {
        Task<ServiceResponse> GetAsync();
        Task<ServiceResponse> CreateAsync(CategoryCreateDTO categoryCreateDTO);
        Task<ServiceResponse> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO);
        Task<ServiceResponse> DeleteAsync(int id);
    }
}
