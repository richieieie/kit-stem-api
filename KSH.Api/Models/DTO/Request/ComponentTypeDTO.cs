using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO
{
    public class ComponentTypeDTO
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
