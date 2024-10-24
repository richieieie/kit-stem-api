using System.ComponentModel.DataAnnotations.Schema;

namespace KSH.Api.Models.Domain
{
    [Table("ShippingFee")]
    public class ShippingFee
    {
        public int Id { get; set; }
        public int FromDistance { get; set; }
        public int ToDistance { get; set; }
        public long Price { get; set; }
    }
}