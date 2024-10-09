using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO.Request
{
    public class PaymentCreateDTO
    {
        [Required]
        public Guid OrderId { get; set; }
    }
}