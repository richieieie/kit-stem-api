using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
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