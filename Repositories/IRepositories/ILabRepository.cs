using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kit_stem_api.Models.Domain;

namespace kit_stem_api.Repositories.IRepositories
{
    public interface ILabRepository
    {
        Task<bool> CreateAsync(Lab lab);
    }
}