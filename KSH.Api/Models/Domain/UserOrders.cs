﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KSH.Api.Models.Domain
{
    [Table("Order")]
    public partial class UserOrders
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(450)]
        public string UserId { get; set; } = null!;
        public int ShippingFeeId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? DeliveredAt { get; set; }

        public string ShippingStatus { get; set; } = null!;

        public string ShippingAddress { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public bool IsLabDownloaded { get; set; }

        public long Price { get; set; }

        public long Discount { get; set; }

        public long TotalPrice { get; set; }

        public string? Note { get; set; }

        [JsonIgnore]
        [InverseProperty("Order")]
        public virtual ICollection<OrderSupport> OrderSupports { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("Order")]
        public virtual ICollection<PackageOrder> PackageOrders { get; set; } = null!;

        [ForeignKey("UserId")]
        [InverseProperty("UserOrders")]
        public virtual ApplicationUser User { get; set; } = null!;

        [InverseProperty("UserOrders")]
        public virtual Payment? Payment { get; set; }

        [ForeignKey("ShippingFeeId")]
        public virtual ShippingFee ShippingFee { get; set; } = null!;
    }
}