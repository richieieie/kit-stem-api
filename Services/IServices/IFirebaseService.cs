using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kit_stem_api.Services.IServices
{
    public interface IFirebaseService
    {
        Task<ServiceResponse> UploadFileAsync(string bucket, string folder, string fileName, IFormFile file);
    }
}