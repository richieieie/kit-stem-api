using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kit_stem_api.Models.Domain;

namespace kit_stem_api.Models.DTO.Response
{
    public class OrderResponseDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? DeliveredAt { get; set; }
        public string ShippingStatus { get; set; } = null!;
        public bool IsLabDownloaded { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int TotalPrice { get; set; }
        public string? Note { get; set; }
        public ICollection<PackageOrderResponseDTO>? PackageOrders { get; set; }
    }
}