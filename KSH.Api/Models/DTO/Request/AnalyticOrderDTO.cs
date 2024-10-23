namespace KSH.Api.Models.DTO.Request
{
    public class AnalyticOrderDTO
    {
        public DateTimeOffset FromDate { get; set; } = DateTimeOffset.MinValue;
        public DateTimeOffset ToDate { get; set; } = DateTimeOffset.MaxValue;
        public string? ShippingStatus { get; set; }
    }
}