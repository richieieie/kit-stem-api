using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KSH.Api.Models.Domain
{

    [Table("Type")]
    public class ComponentsType
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;
        public bool Status { get; set; }
        [JsonIgnore]
        [InverseProperty("Type")]
        public virtual ICollection<Component>? Components { get; set; }
    }
}