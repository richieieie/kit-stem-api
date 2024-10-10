using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KST.Api.Models.Domain
{

    [PrimaryKey("LabId")]
    [Table("OrderSupport")]
    public class OrderSupport
    {
        [Key]
        public Guid Id { get; set; }
        public Guid LabId { get; set; }
        public Guid OrderId { get; set; }
        public int PackageId { get; set; }

        public int RemainSupportTimes { get; set; }

        [ForeignKey("LabId")]
        [InverseProperty("OrderSupports")]
        public virtual Lab Lab { get; set; } = null!;

        [ForeignKey("OrderId")]
        [InverseProperty("OrderSupports")]
        public virtual UserOrders Order { get; set; } = null!;

        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; } = null!;

        [InverseProperty("OrderSupport")]
        public virtual ICollection<LabSupport> LabSupports { get; set; } = null!;
    }
}