using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO
{
    public class UserUpdateDTO
    {
        public string? FirstName { get; set; }
        [MaxLength(45)]
        public string? LastName { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }
    }
}
