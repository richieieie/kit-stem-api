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
        public Guid LabId { get; set; }

        [Key]
        public Guid OrderId { get; set; }

        public int RemainSupportTimes { get; set; }

        [ForeignKey("LabId")]
        [InverseProperty("LabSupports")]
        public virtual Lab Lab { get; set; } = null!;

        [ForeignKey("OrderId")]
        [InverseProperty("LabSupports")]
        public virtual UserOrders Order { get; set; } = null!;
    }
}