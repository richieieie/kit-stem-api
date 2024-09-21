using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kit_stem_api.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace kit_stem_api.Repositories.IRepositories
{
    public interface ITokenRepository
    {
        string GenerateJwtToken(ApplicationUser user, string role);
        Task<RefreshToken> CreateOrUpdateRefreshTokenAsync(ApplicationUser user);
        Task<string?> GetUserIdByRefreshTokenAsync(Guid refreshTokenId);
    }
}