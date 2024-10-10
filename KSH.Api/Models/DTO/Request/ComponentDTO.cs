using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO
{
    public class ComponentDTO
    {
        public int Id { get; set; }

        public int TypeId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        public bool Status { get; set; }
    }
}
