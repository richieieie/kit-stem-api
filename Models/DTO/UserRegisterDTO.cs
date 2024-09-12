using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO
{
    public class UserRegisterDTO
    {
        [EmailAddress(ErrorMessage = "Username must be in email format!")]
        public string? Username { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long, and contain an uppercase letter, a lowercase letter, a number, and a special character.")]
        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; }
    }
}