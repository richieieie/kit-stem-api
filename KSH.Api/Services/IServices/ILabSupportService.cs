using KSH.Api.Services;
using KST.Api.Models.DTO.Request;
using System.Threading.Tasks;
using System;

namespace KST.Api.Services.IServices
{
    public interface ILabSupportService
    {
        Task<ServiceResponse> GetAsync(LabSupportGetDTO getDTO);
        Task<ServiceResponse> GetByCustomerId(String userId);
        Task<ServiceResponse> CreateAsync(string orderId, int packageId, string labId);
        Task<ServiceResponse> UpdateStaffAsync(String staffId, Guid labSupportId);
        Task<ServiceResponse> UpdateFinishedAsync(Guid labSupportId);
        Task<ServiceResponse> UpdateReviewAsync(LabSupportReviewUpdateDTO DTO);
    }
}
