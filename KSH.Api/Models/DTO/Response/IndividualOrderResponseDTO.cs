namespace KSH.Api.Models.DTO.Response
{
    public class IndividualOrderResponseDTO
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? DeliveredAt { get; set; }
        public string? ShippingStatus { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsLabDownloaded { get; set; }
        public long Price { get; set; }
        public long Discount { get; set; }
        public long ShippingFee { get; set; }
        public long TotalPrice { get; set; }
        public string? Note { get; set; }
        public UserProfileDTO? User { get; set; }
        public List<OrderSupportResponseDTO>? OrderSupports { get; set; }
        public List<PackageOrderResponseDTO>? PackageOrders { get; set; }
    }
}