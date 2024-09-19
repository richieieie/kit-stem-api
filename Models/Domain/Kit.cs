using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{

    [Table("Kit")]
    public class Kit
    {
        [Key]
        public int Id { get; set; }

        public int? CategoryId { get; set; }

        [StringLength(100)]
        public string? Name { get; set; }

        [StringLength(255)]
        public string? Brief { get; set; }

        public string? Description { get; set; }

        public int PurchaseCost { get; set; }

        public int Price { get; set; }

        public bool? Status { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Kits")]
        public virtual KitsCategory? Category { get; set; }

        [InverseProperty("Kit")]
        public virtual ICollection<KitComponent> KitComponents { get; set; } = new List<KitComponent>();

        [InverseProperty("Kit")]
        public virtual ICollection<KitImage> KitImages { get; set; } = new List<KitImage>();

        [InverseProperty("Kit")]
        public virtual ICollection<Lab> Labs { get; set; } = new List<Lab>();

        [InverseProperty("Kit")]
        public virtual ICollection<Package> Packages { get; set; } = new List<Package>();
    }
}