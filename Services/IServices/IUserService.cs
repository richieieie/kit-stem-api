using Google.Apis.Auth;
using kit_stem_api.Models.DTO;
using kit_stem_api.Models.DTO.Request;

namespace kit_stem_api.Services.IServices
{
    public interface IUserService
    {
        Task<ServiceResponse> RegisterAsync(UserRegisterDTO requestBody, string role);
        Task<ServiceResponse> LoginAsync(UserLoginDTO requestBody);
        Task<ServiceResponse> GetAllAsync(UserManagerGetDTO userManagerGetDTO);
        Task<ServiceResponse> GetAsync(string userName);
        Task<ServiceResponse> UpdateAsync(string userName, UserUpdateDTO userUpdateDTO);
        Task<ServiceResponse> RefreshTokenAsync(Guid refreshTokenReq);
        Task<ServiceResponse> LoginWithGoogleAsync(GoogleJsonWebSignature.Payload payload);
        Task<ServiceResponse> RemoveByEmailAsync(string userName);
        Task<ServiceResponse> RestoreByEmailAsync(string userName);
    }
}