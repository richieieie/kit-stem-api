namespace KSH.Api.Models.DTO.Request
{
    public class OrderShippingStatusUpdateDTO
    {
        public Guid Id { get; set; }
        public string? ShippingStatus { get; set; }
    }
}
