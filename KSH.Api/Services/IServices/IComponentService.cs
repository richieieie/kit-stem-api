using KSH.Api.Models.DTO;
using KSH.Api.Models.DTO.Request;

namespace KSH.Api.Services.IServices
{
    public interface IComponentService
    {
        Task<ServiceResponse> GetAsync(ComponentGetDTO componentGetDTO);
        Task<ServiceResponse> GetByIdAsync(int id);
        Task<ServiceResponse> CreateAsync(ComponentCreateDTO component);
        Task<ServiceResponse> UpdateAsync(ComponentUpdateDTO component);
        Task<ServiceResponse> RemoveByIdAsync(int id);
        Task<ServiceResponse> RestoreByIdAsync(int id);

    }
}
