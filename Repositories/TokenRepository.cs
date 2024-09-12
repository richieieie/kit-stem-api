using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using kit_stem_api.Models.Domain;
using kit_stem_api.Repositories.IRepositories;
using Microsoft.IdentityModel.Tokens;

namespace kit_stem_api.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;
        public TokenRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
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
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}