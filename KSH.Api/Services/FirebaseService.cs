using Google.Cloud.Storage.V1;
using KSH.Api.Services.IServices;

namespace KSH.Api.Services
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
            var serviceResponse = new ServiceResponse();
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

                return serviceResponse
                        .SetSucceeded(true)
                        .AddDetail("url", fullFileName);
            }
            catch
            {
                return serviceResponse
                        .SetSucceeded(false)
                        .AddDetail("message", "Tạo mới bài file thất bại")
                        .AddError("outOfService", $"Không thể tạo {file.Name} ngay bây giờ!");
            }
        }
        public async Task<ServiceResponse> UploadFilesAsync(string bucket, string folder, Dictionary<string, IFormFile>? nameFiles)
        {

            var serviceResponse = new ServiceResponse();
            try
            {
                var filePrefix = $"{folder}/";
                await DeleteFileWithUnknownExtensionAsync(bucket, filePrefix);
                // (Hưng) câu lệnh thực hiện xóa đi folder kitId nếu người dùng ko upload file image nào
                if (nameFiles == null)
                {
                    return serviceResponse
                            .SetSucceeded(true);
                }
                //

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

                return serviceResponse
                        .SetSucceeded(true)
                        .AddDetail("urls", urls);
            }
            catch
            {
                return serviceResponse
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
        public async Task<ServiceResponse> DownloadFileAsync(string bucket, string pathToFile)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var stream = new MemoryStream();
                var obj = await _storageClient.DownloadObjectAsync(bucket, pathToFile, stream);
                stream.Position = 0;

                return serviceResponse
                        .SetSucceeded(true)
                        .AddDetail("stream", stream)
                        .AddDetail("contentType", obj.ContentType);
            }
            catch
            {
                return serviceResponse
                        .SetSucceeded(false)
                        .AddDetail("message", "Tải file thất bại")
                        .AddError("outOfService", $"Không thể tải file ngay bây giờ!");
            }
        }
    }
}