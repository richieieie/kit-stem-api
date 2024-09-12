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

        public async Task<(bool, string)> LoginAsync(UserLoginDTO requestBody)
        {
            var user = await _userManager.FindByNameAsync(requestBody.Username);
            if (user == null)
            {
                return (false, "Incorrect username or password!");
            }

            var passwordCorrect = await _userManager.CheckPasswordAsync(user, requestBody.Password);
            if (!passwordCorrect)
            {
                return (false, "Incorrect username or password!");
            }
            var role = (await _userManager.GetRolesAsync(user))[0];
            var token = _tokenRepository.GenerateJwtToken(user, role);

            return (true, token);
        }

        public async Task<(bool, string)> RegisterAsync(UserRegisterDTO requestBody)
        {
            var user = await _userManager.FindByNameAsync(requestBody.Username);
            if (user != null)
            {
                return (false, "Username exists!");
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
                    return (false, "Cannot create any new users right now!");
                }

                identityResult = await _userManager.AddToRoleAsync(user, requestBody.Role);
                if (!identityResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return (false, $"{requestBody.Role} does not exist!");
                }

                await transaction.CommitAsync();
                return (true, "New user was created");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, ex.Message);
            }
        }
    }
}