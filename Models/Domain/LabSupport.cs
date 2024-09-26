using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{

    [PrimaryKey("LabId", "OrderId")]
    [Table("LabSupport")]
    public class LabSupport
    {
        [Key]
        public Guid Id { get; set; }
        public Guid LabId { get; set; }
        public Guid OrderId { get; set; }
        public int PackageId { get; set; }

        public int RemainSupportTimes { get; set; }

        [ForeignKey("LabId")]
        [InverseProperty("LabSupports")]
        public virtual Lab Lab { get; set; } = null!;

        [ForeignKey("OrderId")]
        [InverseProperty("LabSupports")]
        public virtual UserOrders Order { get; set; } = null!;

        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; } = null!;

        [InverseProperty("LabSupport")]
        public virtual ICollection<LabSupporter> LabSupporters { get; set; } = null!;
    }
}