﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace KSH.Api.Models.Domain
{

    [Table("Kit")]
    public class Kit
    {
        [Key]
        public int Id { get; set; }

        public int CategoryId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        [StringLength(255)]
        public string Brief { get; set; } = null!;

        public string Description { get; set; } = null!;
        public long MinPackagePrice { get; set; }
        public long MaxPackagePrice { get; set; }

        public long PurchaseCost { get; set; }

        public bool Status { get; set; }

        [ForeignKey("CategoryId")]
        [InverseProperty("Kits")]
        public virtual KitsCategory Category { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("Kit")]
        public virtual ICollection<KitComponent>? KitComponents { get; set; }

        [JsonIgnore]
        [InverseProperty("Kit")]
        public virtual ICollection<KitImage>? KitImages { get; set; }

        [JsonIgnore]
        [InverseProperty("Kit")]
        public virtual ICollection<Lab>? Labs { get; set; }

        [JsonIgnore]
        [InverseProperty("Kit")]
        public virtual ICollection<Package>? Packages { get; set; }
    }
}