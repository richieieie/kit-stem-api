using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using kit_stem_api.Models.DTO;
using kit_stem_api.Services.IServices;

namespace kit_stem_api.Services
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