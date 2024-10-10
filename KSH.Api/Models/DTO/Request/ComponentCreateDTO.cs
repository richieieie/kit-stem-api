using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO
{
    public class ComponentCreateDTO
    {
        public int TypeId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
