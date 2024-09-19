using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{

    [Table("Lab")]
    public class Lab
    {
        [Key]
        public Guid Id { get; set; }

        public int LevelId { get; set; }

        public int KitId { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [Unicode(false)]
        public string Url { get; set; } = null!;

        public int Price { get; set; }

        public int MaxSupportTimes { get; set; }

        [StringLength(100)]
        public string? Author { get; set; }

        public bool Status { get; set; }

        [ForeignKey("KitId")]
        [InverseProperty("Labs")]
        public virtual Kit Kit { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("Lab")]
        public virtual ICollection<LabSupport>? LabSupports { get; set; }

        [ForeignKey("LevelId")]
        [InverseProperty("Labs")]
        public virtual Level Level { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("Lab")]
        public virtual ICollection<PackageLab>? PackageLabs { get; set; }
    }
}