using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{

    [Table("Package")]
    public class Package
    {
        [Key]
        public int Id { get; set; }

        public int KitId { get; set; }

        public int Price { get; set; }

        [ForeignKey("KitId")]
        [InverseProperty("Packages")]
        public virtual Kit Kit { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("Package")]
        public virtual ICollection<PackageLab>? PackageLabs { get; set; }
    }
}