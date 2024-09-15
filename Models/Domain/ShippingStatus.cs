using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{

    [Table("ShippingStatus")]
    public partial class ShippingStatus
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(100)]
        public string NormalizedName { get; set; } = null!;

        [InverseProperty("ShippingStatus")]
        public virtual ICollection<UserOrders> UserOrders { get; set; } = new List<UserOrders>();
    }
}