namespace KSH.Api.Services.IServices
{
    public interface IFirebaseService
    {
        Task<ServiceResponse> UploadFileAsync(string bucket, string folder, string fileName, IFormFile file);
        Task<ServiceResponse> UploadFilesAsync(string bucket, string folder, Dictionary<string, IFormFile>? nameFiles);
        Task<ServiceResponse> DownloadFileAsync(string bucket, string pathToFile);
        ServiceResponse GetSignedUrlAsync(string bucket, string filePath, int expirationInMinutes = 15);
    }
}