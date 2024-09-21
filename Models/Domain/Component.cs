using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{

    [Table("Component")]
    public class Component
    {
        [Key]
        public int Id { get; set; }

        public int TypeId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        [InverseProperty("Component")]
        public virtual ICollection<KitComponent>? KitComponents { get; set; }

        [ForeignKey("TypeId")]
        [InverseProperty("Components")]
        public virtual ComponentsType? Type { get; set; }
    }
}