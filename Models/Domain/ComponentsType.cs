using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{

    [Table("ComponentsType")]
    public class ComponentsType
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        [InverseProperty("Type")]
        public virtual ICollection<Component> Components { get; set; } = new List<Component>();
    }
}