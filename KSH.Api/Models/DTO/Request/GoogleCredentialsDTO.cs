using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KSH.Api.Models.DTO
{
    public class GoogleCredentialsDTO
    {
        public string? PendingToken { get; set; }
        public string? IdToken { get; set; }
        public string? AccessToken { get; set; }
    }
}