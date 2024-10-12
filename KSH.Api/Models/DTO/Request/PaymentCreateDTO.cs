using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class PaymentCreateDTO
    {
        [Required]
        public Guid OrderId { get; set; }
    }
}