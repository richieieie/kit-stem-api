using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class LabDTO
    {
        public Guid Id { get; set; }
        public long PurchaseCost { get; set; }
        public int Quantity { get; set; }
    }
}
