using System.ComponentModel.DataAnnotations.Schema;

namespace kit_stem_api.Models.Domain
{
    [Table("Cart")]
    public class Cart
    {
        public string UserId { get; set; } = null!;
        public int PackageId { get; set; }
        public int PackageQuantity { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("Carts")]
        public virtual ApplicationUser User { get; set; } = null!;
        [ForeignKey("PackageId")]
        [InverseProperty("Carts")]
        public virtual Package Package { get; set; } = null!;
    }
}