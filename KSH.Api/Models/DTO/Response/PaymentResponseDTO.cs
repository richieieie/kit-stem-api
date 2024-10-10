using KST.Api.Models.Domain;

namespace KST.Api.Models.DTO.Response
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