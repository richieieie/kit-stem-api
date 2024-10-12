namespace KSH.Api.Models.DTO.Request
{
    public class VNPaymentRequestDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public long Amount { get; set; }
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? Status { get; set; } = "0";

        public long PaymentTranId { get; set; }
        public string? BankCode { get; set; }
        public string? PayStatus { get; set; }
    }
}