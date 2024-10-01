using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace kit_stem_api.Models.Domain
{
    [Table("LabSupporter")]
    public class LabSupporter
    {
        [Key]
        public Guid Id { get; set; }
        public Guid LabSupportId { get; set; }
        public string StaffId { get; set; } = null!;
        public int Rating { get; set; }
        public string? FeedBack { get; set; }
        public bool IsFinished { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        [ForeignKey("LabSupportId")]
        [InverseProperty("LabSupporters")]
        public virtual LabSupport LabSupport { get; set; } = null!;
        [JsonIgnore]
        [ForeignKey("StaffId")]
        [InverseProperty("LabSupporters")]
        public virtual ApplicationUser Staff { get; set; } = null!;
    }
}