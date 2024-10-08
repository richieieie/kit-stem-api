﻿using KST.Api.Models.Domain;
using KST.Api.Models.DTO.Response;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KST.Api.Models.DTO.Request
{
    public class OrderCreateDTO
    {
        public Guid Id { get; set; }

        [StringLength(450)]
        public string UserId { get; set; } = null!;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? DeliveredAt { get; set; }

        public string ShippingStatus { get; set; } = null!;

        public bool IsLabDownloaded { get; set; }

        public int Price { get; set; }

        public int Discount { get; set; }

        public int TotalPrice { get; set; }

        public string? Note { get; set; }

        public ICollection<PackageOrderCreateDTO> PackageOrders { get; set; } = new List<PackageOrderCreateDTO>();

    }
}
