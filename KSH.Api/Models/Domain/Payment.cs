using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KST.Api.Models.Domain
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }
        public int MethodId { get; set; }
        public Guid OrderId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool Status { get; set; }
        public long Amount { get; set; }
        [ForeignKey("MethodId")]
        [InverseProperty("Payments")]
        public virtual Method Method { get; set; } = null!;
        [ForeignKey("OrderId")]
        [InverseProperty("Payment")]
        public virtual UserOrders? UserOrders { get; set; }
    }
}