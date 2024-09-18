using kit_stem_api.Models.DTO;

namespace kit_stem_api.Services.IServices
{
    public interface IUserService
    {
        Task<ServiceResponse> RegisterAsync(UserRegisterDTO requestBody);
        Task<ServiceResponse> LoginAsync(UserLoginDTO requestBody);

        Task<ServiceResponse> GetProfileAsync(string userName);

    }
}