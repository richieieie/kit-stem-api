using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{

    [Table("Level")]
    [Index("Name", Name = "UQ__Level__737584F6684B4625", IsUnique = true)]
    [Index("NormalizedName", Name = "UQ__Level__A93C97B9E3009846", IsUnique = true)]
    public class Level
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(100)]
        public string NormalizedName { get; set; } = null!;

        [InverseProperty("Level")]
        public virtual ICollection<Lab> Labs { get; set; } = new List<Lab>();
    }
}