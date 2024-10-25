using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class KitCreateDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        [Range(1, int.MaxValue, ErrorMessage = "Danh mục phải lớn hơn hoặc bằng 1")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Vui lòng đặt tên cho kit")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "Vui lòng ghi mô tả ngắn cho kit")]
        public string Brief { get; set; } = "";
        [Required(ErrorMessage = "Vui lòng ghi mô tả cho kit")]
        public string Description { get; set; } = "";
        [Required(ErrorMessage = "Vui lòng nhập giá mua")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá mua phải lớn hơn hoặc bằng 0.")]
        public int PurchaseCost { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public bool Status { get; set; } = true;
        [Required(ErrorMessage = "Vui lòng chọn thành phần")]
        public List<int>? ComponentId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng thành phần")]
        public List<int>? ComponentQuantity { get; set; }
        public List<IFormFile>? KitImagesList { get; set; }

    }
}
