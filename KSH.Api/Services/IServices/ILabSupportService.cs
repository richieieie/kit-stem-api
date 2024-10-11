using KST.Api.Models.DTO.Request;

namespace KST.Api.Services.IServices
{
    public interface ILabSupportService
    {
        Task<ServiceResponse> GetAsync(LabSupportGetDTO getDTO);
        //Task<ServiceResponse> GetById(Guid id);
        Task<ServiceResponse> CreateAsync(Guid orderId);
        Task<ServiceResponse> UpdateStaffAsync(String staffId, Guid labSupportId);
    }
}
