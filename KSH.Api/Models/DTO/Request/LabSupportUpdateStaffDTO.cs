using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO.Request
{
    public class LabSupportUpdateStaffDTO
    {
        [Required(ErrorMessage = "Vui lòng cung cấp ID!")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp ID hỗ trợ đơn hàng!")]
        public Guid OrderSupportId { get; set; }
    }
}
