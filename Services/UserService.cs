using Google.Apis.Auth;
using kit_stem_api.Constants;
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
                        .AddDetail("notFound", "Không tìm thấy user ngay lúc này!");
                }

                var userProfileDTO = new UserProfileDTO()
                {
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Points = user.Points
                };

                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy thông tin tài khoản thành công!")
                    .AddDetail("data", new { userProfileDTO });
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Lấy thông tin tài khoản thất bại!")
                    .AddError("outOutService", "Không thể lấy hồ sơ của tài khoản ngay lúc này!");
            }

        }

        public async Task<ServiceResponse> LoginAsync(UserLoginDTO requestBody)
        {
            var user = await _userManager.FindByNameAsync(requestBody.Email!);
            if (user == null || !await _userManager.CheckPasswordAsync(user, requestBody.Password!))
            {
                return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddDetail("message", "Đăng nhập thất bại!")
                            .AddError("invalidCredentials", "Tên đăng nhập hoặc mật khẩu không chính xác!");
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

        public async Task<ServiceResponse> LoginWithGoogleAsync(GoogleJsonWebSignature.Payload payload)
        {
            var email = payload.Email;
            var user = await _userManager.FindByNameAsync(email);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = email,
                    Email = email,
                };
                using var transaction = await _dbContext.Database.BeginTransactionAsync();
                var identityResult = await _userManager.CreateAsync(user);
                if (!identityResult.Succeeded)
                {
                    return new ServiceResponse()
                                .SetSucceeded(false)
                                .AddDetail("message", "Đăng nhập thất bại!")
                                .AddError("outOfService", "Không thể đăng nhập bằng gmail ngay lúc này!");
                }

                identityResult = await _userManager.AddToRoleAsync(user, UserConstants.CustomerRole);
                if (!identityResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ServiceResponse()
                                .SetSucceeded(false)
                                .AddDetail("message", "Tạo tài khoản thất bại!")
                                .AddError("invalidCredentials", "Vai trò yêu cầu không tồn tại!");
                }
                await transaction.CommitAsync();
            }

            var refreshToken = (await _tokenRepository.CreateOrUpdateRefreshTokenAsync(user)).Id;
            var accessToken = _tokenRepository.GenerateJwtToken(user, UserConstants.CustomerRole);
            return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Đăng nhập thành công!")
                    .AddDetail("accessToken", accessToken)
                    .AddDetail("refreshToken", refreshToken);
        }

        public async Task<ServiceResponse> RegisterAsync(UserRegisterDTO requestBody, string role)
        {
            var user = await _userManager.FindByNameAsync(requestBody.Email!);
            if (user != null)
            {
                return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddDetail("message", "Tạo tài khoản thất bại")
                            .AddError("unavailableUsername", "Tên tài khoản đã tồn tại!");
            }
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                user = new ApplicationUser()
                {
                    UserName = requestBody.Email,
                    Email = requestBody.Email
                };
                var identityResult = await _userManager.CreateAsync(user, requestBody.Password!);
                if (!identityResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddDetail("message", "Tạo tài khoản thất bại")
                            .AddError("unavailableUsername", "Tên tài khoản đã tồn tại!");
                }

                identityResult = await _userManager.AddToRoleAsync(user, role);
                if (!identityResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ServiceResponse()
                                .SetSucceeded(false)
                                .AddDetail("message", "Tạo tài khoản thất bại!")
                                .AddError("invalidCredentials", "Vai trò yêu cầu không tồn tại!");
                }

                await transaction.CommitAsync();
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("message", "Tạo mới tài khoản thành công!");
            }
            catch
            {
                await transaction.RollbackAsync();
                return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddDetail("message", "Tạo tài khoản thất bại!")
                            .AddError("outOfService", "Không thể tạo tài khoản ngay lúc này!");
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
                        .AddDetail("message", "Chỉnh sửa thông tin tài khoản thất bại!")
                        .AddError("notFound", "Không thể chỉnh sửa thông tin tài khoản ngay lúc này!");
                }
                user.FirstName = userUpdateDTO.FirstName;
                user.LastName = userUpdateDTO.LastName;
                user.Address = userUpdateDTO.Address;

                await _userManager.UpdateAsync(user);
                return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Chỉnh sửa thông tin tài khoản thành công!");

            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Chỉnh sửa thông tin tài khoản thất bại!")
                    .AddError("invalidCredentials", "Token yêu cầu đã hết hạn hoặc không hợp lệ");
            }
        }

        public async Task<ServiceResponse> RefreshTokenAsync(Guid refreshTokenReq)
        {
            var userId = await _tokenRepository.GetUserIdByRefreshTokenAsync(refreshTokenReq);
            if (userId == null)
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Làm mới phiên đăng nhập không thành công!")
                        .AddError("invalidCredentials", "Token yêu cầu đã hết hạn hoặc không hợp lệ!");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Làm mới phiên đăng nhập không thành công!")
                        .AddError("invalidCredentials", "Token yêu cầu đã hết hạn hoặc không hợp lệ!");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var refreshToken = (await _tokenRepository.CreateOrUpdateRefreshTokenAsync(user)).Id;
            var accessToken = _tokenRepository.GenerateJwtToken(user, roles.FirstOrDefault()!);
            return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Làm mới phiên đăng nhập thành công!")
                    .AddDetail("accessToken", accessToken)
                    .AddDetail("refreshToken", refreshToken);
        }
    }
}