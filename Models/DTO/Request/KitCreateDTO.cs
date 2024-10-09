using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO.Request
{
    public class KitCreateDTO
    {
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Không tìm thấy sản phẩm")]
        public int CategoryId { get; set; } = 1;
        [Required(ErrorMessage = "phải đặt tên cho kit")]
        [StringLength(100)]
        public string Name { get; set; } = "test";
        [Required(ErrorMessage = "phải ghi mô tả ngắn cho kit")]
        [StringLength(255)]
        public string Brief { get; set; } = "test";
        [Required]
        public string Description { get; set; } = "";
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Giá mua phải lớn hơn hoặc bằng 0.")]
        public int PurchaseCost { get; set; } = 1;
        public List<IFormFile> KitImages { get; set; }
        [Required]
        public bool Status { get; set; } = true;
    }
}
