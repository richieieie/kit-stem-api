using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO
{
    public class KitUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [StringLength(100)]
        public string Name { get; set; } = null!;
        [StringLength(255)]
        public string Brief { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PurchaseCost { get; set; }
        public bool Status { get; set; }
    }
}
