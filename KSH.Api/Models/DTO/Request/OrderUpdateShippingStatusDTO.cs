namespace KST.Api.Models.DTO.Request
{
    public class OrderUpdateShippingStatusDTO
    {
        public Guid Id { get; set; }
        public string? ShippingStatus { get; set; }
    }
}
