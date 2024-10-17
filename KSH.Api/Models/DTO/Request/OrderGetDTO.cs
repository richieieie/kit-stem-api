namespace KSH.Api.Models.DTO.Request
{
    public class OrderGetDTO
    {
        public int Page { get; set; }
        public long FromAmount { get; set; }
        public long ToAmount { get; set; } = long.MaxValue;
        public DateTimeOffset CreatedFrom { get; set; } = DateTimeOffset.MinValue;
        public DateTimeOffset CreatedTo { get; set; } = DateTimeOffset.MaxValue;
        public string? ShippingStatus { get; set; }
        public string? SortFields { get; set; }
        public string? SortOrders { get; set; }
    }
}