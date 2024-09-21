using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace kit_stem_api.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly int refreshTokenExpirationTime = 60 * 60 * 24 * 30;
        private readonly int accessTokenExpirationTime = 60 * 15;
        private readonly IConfiguration _configuration;
        private readonly KitStemDbContext _dbContext;
        public TokenRepository(IConfiguration configuration, KitStemDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }
        public string GenerateJwtToken(ApplicationUser user, string role)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.UserName ?? throw new ArgumentException("Username cannot be null")),
                new Claim(ClaimTypes.Role, role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new ArgumentException("Key cannot be null")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddSeconds(accessTokenExpirationTime),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<RefreshToken> CreateOrUpdateRefreshTokenAsync(ApplicationUser user)
        {
            var rt = await GetRefreshTokenAsync(user.Id);
            if (rt != null)
            {
                _dbContext.RefreshTokens.Remove(rt);
            }

            var newRt = CreateNewRefreshToken(user);
            await _dbContext.RefreshTokens.AddAsync(newRt);
            await _dbContext.SaveChangesAsync();

            return newRt;
        }

        private RefreshToken CreateNewRefreshToken(ApplicationUser user)
        {
            return new RefreshToken()
            {
                UserId = user.Id,
                ExpirationTime = DateTime.Now.AddSeconds(refreshTokenExpirationTime)
            };
        }

        private async Task<RefreshToken?> GetRefreshTokenAsync(string userId)
        {
            return await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserId == userId);
        }

        public async Task<string?> GetUserIdByRefreshTokenAsync(Guid refreshTokenId)
        {
            var rt = await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Id == refreshTokenId);
            if (rt == null || rt.ExpirationTime < DateTime.Now)
            {
                return null;
            }

            return rt.UserId;
        }
    }
}