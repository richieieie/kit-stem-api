using KSH.Api.Models.Domain;

namespace KSH.Api.Models.DTO.Response
{
    public class PaymentResponseDTO
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool Status { get; set; }
        public long Amount { get; set; }
        public virtual Method Method { get; set; } = null!;
    }
}