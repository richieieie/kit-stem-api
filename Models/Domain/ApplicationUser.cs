using Microsoft.AspNetCore.Identity;

namespace kit_stem_api.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}