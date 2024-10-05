namespace kit_stem_api.Services.IServices
{
    public interface IFirebaseService
    {
        Task<ServiceResponse> UploadFileAsync(string bucket, string folder, string fileName, IFormFile file);
        Task<ServiceResponse> UploadFilesAsync(string bucket, string folder, Dictionary<string, IFormFile>? nameFiles);
    }
}