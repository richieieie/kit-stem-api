using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Repositories.IRepositories;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
                            .AddDetail("invalidCredentials", "Incorrect username or password!");
            }

            var role = (await _userManager.GetRolesAsync(user))[0];
            var token = _tokenRepository.GenerateJwtToken(user, role);

            return new ServiceResponse()
                        .SetSucceeded(true)
                        .AddDetail("at", token);
        }

        public async Task<ServiceResponse> RegisterAsync(UserRegisterDTO requestBody)
        {
            var user = await _userManager.FindByNameAsync(requestBody.Username);
            if (user != null)
            {
                return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddDetail("unavailableUsername", "Username exists!");
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
                                .AddDetail("outOfService", identityResult.Errors);
                }

                identityResult = await _userManager.AddToRoleAsync(user, requestBody.Role);
                if (!identityResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return new ServiceResponse()
                                .SetSucceeded(false)
                                .AddDetail("invalidCredentials", identityResult.Errors);
                }

                await transaction.CommitAsync();
                return new ServiceResponse()
                            .SetSucceeded(true)
                            .AddDetail("message", "New user was created!");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ServiceResponse()
                            .SetSucceeded(false)
                            .AddDetail("unhandledException", ex.InnerException?.Message ?? ex.Message);
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
    }
}