using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace KSH.Api.Models.Domain
{

    [PrimaryKey("KitId", "ComponentId")]
    [Table("KitComponent")]
    public class KitComponent
    {
        [Key]
        public int KitId { get; set; }

        [Key]
        public int ComponentId { get; set; }

        public int ComponentQuantity { get; set; }

        [ForeignKey("ComponentId")]
        [InverseProperty("KitComponents")]
        public virtual Component Component { get; set; } = null!;

        [ForeignKey("KitId")]
        [InverseProperty("KitComponents")]
        public virtual Kit Kit { get; set; } = null!;
    }
}