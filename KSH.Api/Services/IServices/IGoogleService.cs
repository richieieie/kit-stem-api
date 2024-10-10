using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KST.Api.Models.DTO;

namespace KST.Api.Services.IServices
{
    public interface IGoogleService
    {
        Task<ServiceResponse> VerifyGoogleTokenAsync(GoogleCredentialsDTO googleCredentialsDTO);
    }
}