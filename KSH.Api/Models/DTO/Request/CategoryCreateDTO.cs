using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO
{
    public class CategoryCreateDTO
    {
        [StringLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
