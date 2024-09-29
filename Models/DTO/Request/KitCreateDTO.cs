using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO.Request
{
    public class KitCreateDTO
    {
        [Required]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "phải đặt tên cho kit")]
        [StringLength(100)]
        public string Name { get; set; }
        [Required(ErrorMessage = "phải ghi mô tả ngắn cho kit")]
        [StringLength(255)]
        public string Brief { get; set; }
        public string Description { get; set; } = "";
        public int PurchaseCost { get; set; }
    }
}
