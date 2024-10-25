using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class KitDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập ID")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập giá mua")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá mua phải lớn hơn hoặc bằng 0")]
        public int PurchaseCost { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
        public int Quantity { get; set; }
    }
}
