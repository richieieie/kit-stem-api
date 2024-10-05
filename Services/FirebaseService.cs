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
            var response = new ServiceResponse();
            try
            {
                var filePrefix = $"{folder}/{fileName}";
                // Try to delete existing file if it exists on google cloud storage
                await DeleteFileWithUnknownExtensionAsync(bucket, filePrefix);

                var ext = Path.GetExtension(file.FileName);
                var fullFileName = $"{filePrefix}{ext}";

                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);


                await _storageClient.UploadObjectAsync(bucket, fullFileName, file.ContentType, stream);

                return response
                        .SetSucceeded(true)
                        .AddDetail("url", fullFileName);
            }
            catch
            {
                return response
                        .SetSucceeded(false)
                        .AddDetail("message", "Tạo mới bài file thất bại")
                        .AddError("outOfService", $"Không thể tạo {file.Name} ngay bây giờ!");
            }
        }
        public async Task<ServiceResponse> UploadFilesAsync(string bucket, string folder, Dictionary<string, IFormFile> nameFiles)
        {
            
            var response = new ServiceResponse();
            try
            {
                var filePrefix = $"{folder}/";
                // Try to delete existing folder if it exists on google cloud storage
                if (nameFiles == null)
                {
                    await DeleteFileWithUnknownExtensionAsync(bucket, filePrefix);
                    return response
                        .SetSucceeded(true);
                }
                await DeleteFileWithUnknownExtensionAsync(bucket, filePrefix);

                var urls = new List<string>();
                foreach (KeyValuePair<string, IFormFile> entry in nameFiles)
                {
                    var file = entry.Value;
                    var ext = Path.GetExtension(file.FileName);
                    var fullFileName = $"{filePrefix}{entry.Key}{ext}";

                    using var stream = new MemoryStream();
                    await file.CopyToAsync(stream);

                    var obj = await _storageClient.UploadObjectAsync(bucket, fullFileName, file.ContentType, stream);
                    urls.Add($"https://storage.googleapis.com/{obj.Bucket}/{obj.Name}");
                }

                return response
                        .SetSucceeded(true)
                        .AddDetail("urls", urls);
            }
            catch
            {
                return response
                        .SetSucceeded(false)
                        .AddDetail("message", "Tạo mới files thất bại")
                        .AddError("outOfService", $"Không thể tạo files ngay bây giờ!");
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