using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{
    public partial class UserOrders
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(450)]
        public string UserId { get; set; } = null!;

        public Guid PaymentId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? DeliveredAt { get; set; }

        public string ShippingStatus { get; set; } = null!;

        public bool IsLabDownloaded { get; set; }

        public int Price { get; set; }

        public int Discount { get; set; }

        public int TotalPrice { get; set; }

        public string? Note { get; set; }

        [JsonIgnore]
        [InverseProperty("Order")]
        public virtual ICollection<OrderSupport>? OrderSupports { get; set; }

        [ForeignKey("PaymentId")]
        [InverseProperty("UserOrders")]
        public virtual Payment Payment { get; set; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("UserOrders")]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}