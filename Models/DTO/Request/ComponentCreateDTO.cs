using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO
{
    public class ComponentCreateDTO
    {
        public int TypeId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
