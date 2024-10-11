using Google.Apis.Auth;
using KST.Api.Models.DTO;
using KST.Api.Models.DTO.Request;

namespace KST.Api.Services.IServices
{
    public interface IUserService
    {
        Task<(ServiceResponse, string?)> RegisterAsync(UserRegisterDTO requestBody, string role);
        Task<ServiceResponse> LoginAsync(UserLoginDTO requestBody);
        Task<ServiceResponse> GetAllAsync(UserManagerGetDTO userManagerGetDTO);
        Task<ServiceResponse> GetAsync(string userName);
        Task<ServiceResponse> UpdateAsync(string userName, UserUpdateDTO userUpdateDTO);
        Task<ServiceResponse> RefreshTokenAsync(Guid refreshTokenReq);
        Task<ServiceResponse> LoginWithGoogleAsync(GoogleJsonWebSignature.Payload payload);
        Task<ServiceResponse> RemoveByEmailAsync(string userName);
        Task<ServiceResponse> RestoreByEmailAsync(string userName);
        Task<ServiceResponse> VerifyEmail(string email, string token);
    }
}