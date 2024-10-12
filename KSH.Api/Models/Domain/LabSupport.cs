using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KSH.Api.Models.Domain
{
    [Table("LabSupport")]
    public class LabSupport
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OrderSupportId { get; set; }
        public string? StaffId { get; set; }
        public int Rating { get; set; }
        public string? FeedBack { get; set; }
        public bool IsFinished { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        [ForeignKey("OrderSupportId")]
        [InverseProperty("LabSupports")]
        public virtual OrderSupport OrderSupport { get; set; } = null!;

        [ForeignKey("StaffId")]
        [InverseProperty("LabSupports")]
        public virtual ApplicationUser Staff { get; set; } = null!;
    }
}