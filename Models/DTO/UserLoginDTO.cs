using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO
{
    public class UserLoginDTO
    {
        [EmailAddress(ErrorMessage = "Username must be in email format!")]
        public string? Username { get; set; }
        [Required(ErrorMessage = "Password is required!")]
        public string? Password { get; set; }
    }
}