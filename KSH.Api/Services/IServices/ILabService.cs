using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KST.Api.Models.DTO;

namespace KST.Api.Services.IServices
{
    public interface ILabService
    {
        Task<ServiceResponse> CreateAsync(LabUploadDTO labUploadDTO, Guid id, string url);
        Task<ServiceResponse> UpdateAsync(LabUpdateDTO labUpdateDTO, string? url);
        Task<ServiceResponse> GetAsync(LabGetDTO labGetDTO);
        Task<ServiceResponse> GetByIdAsync(Guid id);
        Task<ServiceResponse> GetByKitId(int kitId);
        Task<ServiceResponse> RemoveByIdAsync(Guid id);
        Task<ServiceResponse> RestoreByIdAsync(Guid id);
        Task<ServiceResponse> GetFileUrlByIdAsync(Guid id);
        Task<ServiceResponse> GetFileUrlByIdAndOrderIdAsync(string userId, Guid labId, Guid orderId);
    }
}