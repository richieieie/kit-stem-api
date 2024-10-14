namespace KSH.Api.Models.DTO.Response
{
    public class OrderResponseDTO
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? DeliveredAt { get; set; }
        public string? ShippingStatus { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsLabDownloaded { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public int TotalPrice { get; set; }
        public string? Note { get; set; }
        public UserProfileDTO? User { get; set; }
        public ICollection<PackageOrderResponseDTO>? PackageOrders { get; set; }
    }
}