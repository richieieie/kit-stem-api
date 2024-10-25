using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class KitDTO
    {  
        public int Id { get; set; }
        public long PurchaseCost { get; set; }
        public int Quantity { get; set; }
    }
}
