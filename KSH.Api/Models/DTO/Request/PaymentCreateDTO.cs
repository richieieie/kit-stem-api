using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO.Request
{
    public class PaymentCreateDTO
    {
        [Required]
        public Guid OrderId { get; set; }
    }
}