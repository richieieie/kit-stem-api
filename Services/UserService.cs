using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories;
using kit_stem_api.Repositories.IRepositories;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace kit_stem_api.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly KitStemDbContext _dbContext;
        public UserService(KitStemDbContext dbContext, UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        public async Task<ServiceResponse> GetProfileAsync(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("notFound", "User not found!");
                }

                var userProfileDTO = new UserProfileDTO()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Points = user.Points
                };

                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("profile", userProfileDTO);
            }
            catch (Exception ex)
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("unhandledException", ex.InnerException?.Message ?? ex.Message);
            }

        }

        public async Task<ServiceResponse> LoginAsync(UserLoginDTO requestBody)
        {
            var user = await _userManager.FindByNameAsync(requestBody.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, requestBody.Password))
            {
                return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddDetail("message", "Đăng nhập thất bại!")
                            .AddDetail("invalidCredentials", "Tên đăng nhập hoặc mật khẩu không chính xác!");
            }

            var role = (await _userManager.GetRolesAsync(user))[0];
            var refreshToken = (await _tokenRepository.CreateOrUpdateRefreshTokenAsync(user)).Id;
            var accessToken = _tokenRepository.GenerateJwtToken(user, role);

            return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Đăng nhập thành công!")
                        .AddDetail("accessToken", accessToken)
                        .AddDetail("refreshToken", refreshToken);
        }

        public async Task<ServiceResponse> RegisterAsync(UserRegisterDTO requestBody)
        {
            var user = await _userManager.FindByNameAsync(requestBody.Username);
            if (user != null)
            {
                return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddDetail("unavailableUsername", "Tên tài khoản đã tồn tại!");
            }
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                user = new ApplicationUser()
                {
                    UserName = requestBody.Username,
                    Email = requestBody.Username
                };
                var identityResult = await _userManager.CreateAsync(user, requestBody.Password);
                if (!identityResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ServiceResponse()
                                .SetSucceeded(false)
                                .AddDetail("message", "Thông tin tài khoản không hợp lệ!")
                                .AddDetail("errors", identityResult.Errors);
                }

                identityResult = await _userManager.AddToRoleAsync(user, "customer");
                if (!identityResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ServiceResponse()
                                .SetSucceeded(false)
                                .AddDetail("message", "Vai trò không tồn tại!")
                                .AddDetail("errors", identityResult.Errors);
                }

                await transaction.CommitAsync();
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("message", "Tạo mới tài khoản thành công!");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddDetail("message", "Không thể tạo tài khoản ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> UpdateProfileAsync(string userName, UserUpdateDTO userUpdateDTO)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("notFound", "User not found!");
                }
                user.FirstName = userUpdateDTO.FirstName;
                user.LastName = userUpdateDTO.LastName;
                user.Address = userUpdateDTO.Address;

                await _userManager.UpdateAsync(user);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("update", user);

            }
            catch (Exception ex)
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("unhandledException", ex.InnerException?.Message ?? ex.Message);
            }
        }

        public async Task<ServiceResponse> RefreshTokenAsync(Guid refreshTokenReq)
        {
            var userId = await _tokenRepository.GetUserIdByRefreshTokenAsync(refreshTokenReq);
            if (userId == null)
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Refresh token hết hạn hoặc không hợp lệ!");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Refresh token hết hạn hoặc không hợp lệ!");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var refreshToken = (await _tokenRepository.CreateOrUpdateRefreshTokenAsync(user)).Id;
            var accessToken = _tokenRepository.GenerateJwtToken(user, roles.FirstOrDefault());
            return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Làm mới phiên đăng nhập thành công!")
                    .AddDetail("accessToken", accessToken)
                    .AddDetail("refreshToken", refreshToken);
        }
    }
}