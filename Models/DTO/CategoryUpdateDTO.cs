using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO
{
    public class CategoryUpdateDTO
    {
        [StringLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
