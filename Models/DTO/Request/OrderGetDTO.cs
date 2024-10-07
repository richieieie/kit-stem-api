using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Models.DTO.Request
{
    public class OrderGetDTO
    {
        public int Page { get; set; }
        public DateTimeOffset CreatedFrom { get; set; } = DateTimeOffset.MinValue;
        public DateTimeOffset CreatedTo { get; set; } = DateTimeOffset.MaxValue;
        public string? CustomerEmail { get; set; }
    }
}