using kit_stem_api.Models.Domain;

namespace kit_stem_api.Models.DTO.Response
{
    public class PaymentResponseDTO
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool Status { get; set; }
        public int Amount { get; set; }
        public virtual Method Method { get; set; } = null!;
    }
}