using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class KitCreateDTO
    {
        [Required]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "phải đặt tên cho kit")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "phải ghi mô tả ngắn cho kit")]
        public string Brief { get; set; } = "";
        [Required]
        public string Description { get; set; } = "";
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Giá mua phải lớn hơn hoặc bằng 0.")]
        public long PurchaseCost { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public List<int>? ComponentId { get; set; }
        [Required]
        public List<int>? ComponentQuantity { get; set; }
        public List<IFormFile>? KitImagesList { get; set; }

    }
}
