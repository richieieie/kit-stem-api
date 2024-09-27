using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{

    [Table("Level")]
    [Index("Name", Name = "UQ__Level__737584F6684B4625", IsUnique = true)]
    public class Level
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;
        public bool Status { get; set; }

        [JsonIgnore]
        [InverseProperty("Level")]
        public virtual ICollection<Lab>? Labs { get; set; }
    }
}