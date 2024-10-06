using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Models.DTO.Request
{
    public class OrderGetDTO
    {
        public int Page { get; set; }
        [FromQuery(Name = "created-from")]
        public DateTimeOffset CreatedFrom { get; set; } = DateTimeOffset.MinValue;
        [FromQuery(Name = "created-to")]
        public DateTimeOffset CreatedTo { get; set; } = DateTimeOffset.MaxValue;
        [FromQuery(Name = "customer-email")]
        public string? CustomerEmail { get; set; }
    }
}