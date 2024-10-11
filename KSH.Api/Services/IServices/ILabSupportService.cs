using KST.Api.Models.DTO.Request;

namespace KST.Api.Services.IServices
{
    public interface ILabSupportService
    {
        Task<ServiceResponse> GetAsync(LabSupportGetDTO getDTO);
        Task<ServiceResponse> GetSupportsAsync(LabSupportGetDTO getDTO);
        Task<ServiceResponse> GetByCustomerId(String userId);

        Task<ServiceResponse> CreateAsync(Guid orderId);
        Task<ServiceResponse> UpdateStaffAsync(String staffId, Guid labSupportId);
        Task<ServiceResponse> UpdateFinishedAsync(Guid labSupportId);
        Task<ServiceResponse> UpdateReviewAsync(LabSupportReviewUpdateDTO DTO);
    }
}
