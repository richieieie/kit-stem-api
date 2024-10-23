using AutoMapper;
using Google.Apis.Auth;
using KSH.Api.Constants;
using KSH.Api.Data;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Repositories.IRepositories;
using KSH.Api.Services.IServices;
using Microsoft.AspNetCore.Identity;

namespace KSH.Api.Services
{
    public class UserService : IUserService
    {
        private readonly int pageSize = 20;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly IMapper _mapper;
        private readonly KitStemDbContext _dbContext;
        public UserService(KitStemDbContext dbContext, UserManager<ApplicationUser> userManager, ITokenRepository tokenRepository, IMapper mapper)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> GetAsync(string userName)
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
                    Points = user.Points,
                    Status = user.Status
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
            var serviceResponse = new ServiceResponse();
            var user = await _userManager.FindByNameAsync(requestBody.Email!);
            if (user == null || !await _userManager.CheckPasswordAsync(user, requestBody.Password!))
            {
                return serviceResponse
                            .SetSucceeded(false)
                            .AddDetail("message", "Đăng nhập thất bại!")
                            .AddError("invalidCredentials", "Tên đăng nhập hoặc mật khẩu không chính xác!");
            }

            if (!user.EmailConfirmed)
            {
                return serviceResponse
                            .SetSucceeded(false)
                            .AddDetail("message", "Đăng nhập thất bại!")
                            .AddError("invalidCredentials", "Vui lòng xác thực tài khoản của bạn thông qua email!");
            }

            if (!user.Status)
            {
                return serviceResponse
                            .SetSucceeded(false)
                            .AddDetail("message", "Đăng nhập thất bại!")
                            .AddError("invalidCredentials", "Tài khoản của bạn đã bị vô hiệu hoá, vui lòng liện hệ của hàng qua số điện thoại 000000000 để được hỗ trợ!");
            }

            var role = (await _userManager.GetRolesAsync(user))[0];
            var refreshToken = (await _tokenRepository.CreateOrUpdateRefreshTokenAsync(user)).Id;
            var accessToken = _tokenRepository.GenerateJwtToken(user, role);

            return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("message", "Đăng nhập thành công!")
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
                    EmailConfirmed = true,
                    Status = true
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

            if (!user.Status)
            {
                return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddDetail("message", "Đăng nhập thất bại!")
                            .AddError("invalidCredentials", "Tài khoản của bạn đã bị vô hiệu hoá, vui lòng liện hệ của hàng qua số điện thoại 000000000 để được hỗ trợ!");
            }

            var refreshToken = (await _tokenRepository.CreateOrUpdateRefreshTokenAsync(user)).Id;
            var accessToken = _tokenRepository.GenerateJwtToken(user, UserConstants.CustomerRole);
            return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Đăng nhập thành công!")
                    .AddDetail("accessToken", accessToken)
                    .AddDetail("refreshToken", refreshToken);
        }

        public async Task<(ServiceResponse, string?)> RegisterAsync(UserRegisterDTO requestBody, string role)
        {
            var serviceResponse = new ServiceResponse();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = new ApplicationUser()
                {
                    UserName = requestBody.Email,
                    Email = requestBody.Email,
                    Status = true
                };
                if (role == UserConstants.StaffRole)
                {
                    user.EmailConfirmed = true;
                }

                var identityResult = await _userManager.CreateAsync(user, requestBody.Password!);
                if (!identityResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return (serviceResponse
                            .SetSucceeded(false)
                            .AddDetail("message", "Tạo tài khoản thất bại!")
                            .AddError("unavailableUsername", "Tên tài khoản đã tồn tại!"),
                            null);
                }

                identityResult = await _userManager.AddToRoleAsync(user, role);
                if (!identityResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return (serviceResponse
                                .SetSucceeded(false)
                                .AddDetail("message", "Tạo tài khoản thất bại!")
                                .AddError("invalidCredentials", "Vai trò yêu cầu không tồn tại!"),
                            null);
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                await transaction.CommitAsync();
                return (serviceResponse
                            .SetSucceeded(true)
                            .AddDetail("message", "Tạo mới tài khoản thành công!"),
                        token);
            }
            catch
            {
                await transaction.RollbackAsync();
                return (serviceResponse
                            .SetSucceeded(false)
                            .AddDetail("message", "Tạo tài khoản thất bại!")
                            .AddError("outOfService", "Không thể tạo tài khoản ngay lúc này!"),
                        null);
            }
        }

        public async Task<ServiceResponse> UpdateAsync(string userName, UserUpdateDTO userUpdateDTO)
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
                user.PhoneNumber = userUpdateDTO.PhoneNumber;

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
            if (user!.Status == false)
            {
                return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Làm mới phiên đăng nhập không thành công!")
                        .AddError("unavailable", "Tài khoản của bạn đã bị vô hiệu hoá, vui lòng liên hệ số điện thoại 000000000 để được hỗ trợ!");
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

        public async Task<ServiceResponse> GetAllAsync(UserManagerGetDTO userManagerGetDTO)
        {

            var usersInRole = (await _userManager.GetUsersInRoleAsync(userManagerGetDTO.Role!)).Where(u => u.Email!.Length > 0);
            var totalUsers = usersInRole.Count();
            var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);
            var usersInPage = usersInRole.Skip(userManagerGetDTO.Page * pageSize).Take(pageSize).ToList();
            var userDTOs = _mapper.Map<IEnumerable<UserProfileDTO>>(usersInPage);

            return new ServiceResponse()
                    .SetSucceeded(true)
                    .AddDetail("message", "Lấy thông tin tài khoản thành công!")
                    .AddDetail("data", new { totalPages, currentPage = userManagerGetDTO.Page, users = userDTOs });
        }

        public async Task<ServiceResponse> RemoveByEmailAsync(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Vô hiệu hoá tài khoản không thành công!")
                        .AddError("notFound", "Tài khoản không tồn tại");
                }

                user.Status = false;
                await _userManager.UpdateAsync(user);

                return new ServiceResponse();
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Vô hiệu hoá tài khoản thất bại!")
                    .AddError("outOutService", "Không thể vô hiệu hoá tài khoản ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> RestoreByEmailAsync(string userName)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    return new ServiceResponse()
                        .SetSucceeded(false)
                        .AddDetail("message", "Khôi phục tài khoản không thành công!")
                        .AddError("notFound", "Tài khoản không tồn tại");
                }

                user.Status = true;
                await _userManager.UpdateAsync(user);

                return new ServiceResponse()
                    .AddDetail("message", "Khôi phục tài khoản thành công!");
            }
            catch
            {
                return new ServiceResponse()
                    .SetSucceeded(false)
                    .AddDetail("message", "Khôi phục tài khoản thất bại!")
                    .AddError("outOutService", "Không thể Khôi phục tài khoản ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> VerifyEmail(string email, string token)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return serviceResponse
                        .SetSucceeded(false)
                        .AddDetail("message", "Xác thực tài khoản thất bại")
                        .AddError("unavailable", "Thông tin cung cấp xác thực không hợp lệ!");
                }

                var identityResult = await _userManager.ConfirmEmailAsync(user, token);
                if (!identityResult.Succeeded)
                {
                    return serviceResponse
                        .SetSucceeded(false)
                        .AddDetail("message", "Xác thực tài khoản thất bại")
                        .AddError("unavailable", "Thông tin cung cấp xác thực không hợp lệ!");
                }

                return serviceResponse
                    .AddDetail("message", "Xác thực tài khoản thành công!");
            }
            catch
            {
                return serviceResponse
                        .SetSucceeded(false)
                        .AddDetail("message", "Xác thực tài khoản thất bại")
                        .AddError("outOutService", "Không thể xác thực tài khoản ngay lúc này!");
            }
        }

        public async Task<(ServiceResponse, string?)> GeneratePasswordResetTokenAsync(string email)
        {
            var serviceResponse = new ServiceResponse();
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return (serviceResponse
                        .AddDetail("message", "Không thể cài đặt lại mật khẩu!")
                        .AddError("notFound", "Không tìm thấy tài khoản của bạn trong hệ thống, vui lòng đăng ký!"),
                        null);
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            return (serviceResponse
                    .AddDetail("message", "Tạo mã cài đặt mật khẩu thành công!"),
                    resetToken);
        }

        public async Task<ServiceResponse> ResetPasswordAsync(PasswordResetDTO passwordResetDTO)
        {
            var serviceResponse = new ServiceResponse();
            var user = await _userManager.FindByEmailAsync(passwordResetDTO.Email!);
            if (user == null)
            {
                return serviceResponse
                    .AddDetail("message", "Không thể đặt lại mật khẩu.")
                    .AddError("unavailable", "Đặt lại mật khẩu không thành công. Vui lòng kiểm tra lại token và thử lại.");
            }

            var identityResult = await _userManager.ResetPasswordAsync(user, passwordResetDTO.Token!, passwordResetDTO.NewPassword!);
            if (!identityResult.Succeeded)
            {
                return serviceResponse
                   .AddDetail("message", "Không thể đặt lại mật khẩu.")
                   .AddError("unavailable", "Đặt lại mật khẩu không thành công. Vui lòng kiểm tra lại token và thử lại.");
            }

            return serviceResponse
                    .AddDetail("message", "Đặt lại mật khẩu thành công. Bạn có thể sử dụng mật khẩu mới để đăng nhập.");
        }
    }
}