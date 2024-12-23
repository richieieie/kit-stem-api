﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KSH.Api.Models.Domain
{

    [Table("Category")]
    public class KitsCategory
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public bool Status { get; set; }

        [JsonIgnore]
        [InverseProperty("Category")]
        public virtual ICollection<Kit>? Kits { get; set; }
    }
}