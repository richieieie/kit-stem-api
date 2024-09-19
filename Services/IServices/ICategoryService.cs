namespace kit_stem_api.Services.IServices
{
    public interface ICategoryService
    {
        Task<ServiceResponse> GetCategoriesAsync();
    }
}
