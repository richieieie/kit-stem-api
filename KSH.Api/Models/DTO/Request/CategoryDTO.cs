using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
