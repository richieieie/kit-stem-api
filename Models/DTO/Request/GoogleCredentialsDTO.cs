using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kit_stem_api.Models.DTO
{
    public class GoogleCredentialsDTO
    {
        public string? PendingToken { get; set; }
        public string? IdToken { get; set; }
        public string? AccessToken { get; set; }
    }
}