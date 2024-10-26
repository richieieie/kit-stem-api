using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO
{
    public class UserUpdateDTO
    {
        [Required]
        [MaxLength(45)]
        public string? FirstName { get; set; }
        [MaxLength(45)]
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }

        public int GenderCode { get; set; }
        public DateTimeOffset BirthDate { get; set; }
    }
}
