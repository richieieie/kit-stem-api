using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{

    [Table("KitsCategory")]
    public class KitsCategory
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<Kit> Kits { get; set; } = new List<Kit>();
    }
}