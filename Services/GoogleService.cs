using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using kit_stem_api.Models.DTO;
using kit_stem_api.Services.IServices;

namespace kit_stem_api.Services
{
    public class GoogleService : IGoogleService
    {
        public async Task<ServiceResponse> VerifyGoogleTokenAsync(GoogleCredentialsDTO googleCredentialsDTO)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { "953447545464-a1bjt1e5tssk8vubepu1831et10jrt56.apps.googleusercontent.com" }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(googleCredentialsDTO.IdToken, settings);
                return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Đăng nhập thành công!")
                        .AddDetail("payload", payload);
            }
            catch (Exception ex)
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Đăng nhập thất bại!")
                        .AddError("outOfService", "Không thể đăng nhập bằng tài khoản google ngay lúc này!");
            }
        }

    }
}