using kit_stem_api.Models.DTO;

namespace kit_stem_api.Services.IServices
{
    public interface IUserService
    {
        Task<(bool, string)> RegisterAsync(UserRegisterDTO requestBody);
        Task<(bool, string)> LoginAsync(UserLoginDTO requestBody);
    }
}