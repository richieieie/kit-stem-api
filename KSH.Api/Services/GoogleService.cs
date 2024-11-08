using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using KSH.Api.Models.DTO;
using KSH.Api.Services.IServices;

namespace KSH.Api.Services
{
    public class GoogleService : IGoogleService
    {
        private readonly IConfiguration _configuration;
        public GoogleService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<ServiceResponse> VerifyGoogleTokenAsync(GoogleCredentialsDTO googleCredentialsDTO)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { _configuration["Google:ClientId"]! }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(googleCredentialsDTO.IdToken, settings);
                payload.GivenName = googleCredentialsDTO.FirstName;
                payload.FamilyName = googleCredentialsDTO.LastName;
                return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Đăng nhập thành công!")
                        .AddDetail("payload", payload);
            }
            catch
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Đăng nhập thất bại!")
                        .AddError("outOfService", "Không thể đăng nhập bằng tài khoản google ngay lúc này!");
            }
        }

    }
}