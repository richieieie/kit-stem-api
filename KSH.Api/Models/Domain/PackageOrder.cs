using System.ComponentModel.DataAnnotations.Schema;

namespace KSH.Api.Models.Domain
{
    [Table("PackageOrder")]
    public class PackageOrder
    {
        public int PackageId { get; set; }

        public Guid OrderId { get; set; }

        public int PackageQuantity { get; set; }

        [ForeignKey("OrderId")]
        public virtual UserOrders Order { get; set; } = null!;

        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; } = null!;
    }
}