using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class OrderShippingStatusUpdateDTO
    {
        [Required(ErrorMessage = "Vui lòng cung cấp Id của đơn hàng!")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp trạng thái vận chuyển!")]
        public string? ShippingStatus { get; set; }
    }
}
