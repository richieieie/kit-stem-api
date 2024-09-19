using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kit_stem_api.Models.DTO;

namespace kit_stem_api.Services.IServices
{
    public interface ILabService
    {
        Task<ServiceResponse> CreateAsync(LabUploadDTO labUploadDTO, string url);
    }
}