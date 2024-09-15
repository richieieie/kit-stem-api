using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kit_stem_api.Models.Domain;

namespace kit_stem_api.Repositories.IRepositories
{
    public interface ITokenRepository
    {
        string GenerateJwtToken(ApplicationUser user, string role);
    }
}