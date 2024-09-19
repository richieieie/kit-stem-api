using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using kit_stem_api.Services.IServices;

namespace kit_stem_api.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly StorageClient _storageClient;
        public FirebaseService(StorageClient storageClient)
        {
            _storageClient = storageClient;
        }
        public async Task<ServiceResponse> UploadFileAsync(string bucket, string folder, string fileName, IFormFile file)
        {
            try
            {
                var fullFileName = $"{folder}/{fileName}{Path.GetExtension(file.FileName)}";
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var obj = await _storageClient.UploadObjectAsync(bucket, fullFileName, file.ContentType, stream);

                return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("url", fullFileName);
            }
            catch (Exception ex)
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("exception", ex.Message)
                        .AddDetail("unhandledException", $"Cannot upload {file.Name} right now!");
            }
        }
    }
}