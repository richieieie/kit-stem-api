using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KST.Api.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace KST.Api.Repositories.IRepositories
{
    public interface ITokenRepository
    {
        string GenerateJwtToken(ApplicationUser user, string role);
        Task<RefreshToken> CreateOrUpdateRefreshTokenAsync(ApplicationUser user);
        Task<string?> GetUserIdByRefreshTokenAsync(Guid refreshTokenId);
    }
}