using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KSH.Api.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace KSH.Api.Repositories.IRepositories
{
    public interface ITokenRepository
    {
        string GenerateJwtToken(ApplicationUser user, string role);
        Task<RefreshToken> CreateOrUpdateRefreshTokenAsync(ApplicationUser user);
        Task<string?> GetUserIdByRefreshTokenAsync(Guid refreshTokenId);
    }
}