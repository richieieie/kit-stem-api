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
                var filePrefix = $"{folder}/{fileName}";
                var ext = Path.GetExtension(file.FileName);
                var fullFileName = $"{filePrefix}{ext}";

                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);

                // Try to delete lab's file if it exists on google cloud storage
                await DeleteFileWithUnknownExtensionAsync(bucket, filePrefix);

                await _storageClient.UploadObjectAsync(bucket, fullFileName, file.ContentType, stream);
                return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("url", fullFileName);
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Tạo mới bài Lab thất bại")
                        .AddError("outOfService", $"Không thể tạo {file.Name} ngay bây giờ!");
            }
        }

        private async Task DeleteFileWithUnknownExtensionAsync(string bucket, string filePrefix)
        {
            try
            {
                var objects = _storageClient.ListObjectsAsync(bucket, filePrefix);

                await foreach (var obj in objects)
                {
                    if (obj.Name.StartsWith(filePrefix))
                    {
                        await _storageClient.DeleteObjectAsync(bucket, obj.Name);
                        break;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}