using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KSH.Api.Models.DTO;

namespace KSH.Api.Services.IServices
{
    public interface IGoogleService
    {
        Task<ServiceResponse> VerifyGoogleTokenAsync(GoogleCredentialsDTO googleCredentialsDTO);
    }
}