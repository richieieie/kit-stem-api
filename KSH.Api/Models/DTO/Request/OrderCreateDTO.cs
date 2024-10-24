using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Response;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KSH.Api.Models.DTO.Request
{
    public class OrderCreateDTO
    {
        public Guid Id { get; set; }

        [StringLength(450)]
        public string UserId { get; set; } = null!;

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset? DeliveredAt { get; set; }

        public string ShippingStatus { get; set; } = null!;

        public string ShippingAddress { get; set; }= null!;
        public int ShippingFeeId { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public bool IsLabDownloaded { get; set; }

        public int Price { get; set; }

        public int Discount { get; set; }

        public int TotalPrice { get; set; }

        public string? Note { get; set; }

        public ICollection<PackageOrderCreateDTO> PackageOrders { get; set; } = new List<PackageOrderCreateDTO>();

    }
}
