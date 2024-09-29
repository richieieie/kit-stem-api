using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{
    [Table("Image")]
    public class KitImage
    {
        [Key]
        public Guid Id { get; set; }

        public int KitId { get; set; }

        [Unicode(false)]
        public string Url { get; set; } = null!;

        [ForeignKey("KitId")]
        [InverseProperty("KitImages")]
        public virtual Kit Kit { get; set; } = null!;
    }
}