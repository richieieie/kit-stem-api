using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class LabDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập ID")]
        public Guid Id { get; set; } 
        public int PurchaseCost { get; set; } 
        public int Quantity { get; set; }
    }
}
