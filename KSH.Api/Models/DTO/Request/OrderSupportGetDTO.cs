using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO.Request
{
    public class OrderSupportGetDTO
    {
        [Range(0, int.MaxValue, ErrorMessage = "Số trang phải lớn hơn hoặc bằng 0")]
        public int Page {  get; set; } = 0;
    }
}
