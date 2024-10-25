using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class KitUpdateDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập ID")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Tên không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string Name { get; set; } = "";
        [Required(ErrorMessage = "Tóm tắt không được để trống.")]
        public string Brief { get; set; } = "";
        [Required(ErrorMessage = "Mô tả không được để trống.")]
        public string? Description { get; set; } = "";
        [Required(ErrorMessage = "Vui lòng nhập giá mua")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá mua phải lớn hơn hoặc bằng 0.")]
        public int PurchaseCost { get; set; }
        public List<int>? ComponentId { get; set; }
        public List<int>? ComponentQuantity { get; set; }
        public List<IFormFile>? KitImagesList { get; set; }
    }
}
