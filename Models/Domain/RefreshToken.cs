using System.ComponentModel.DataAnnotations.Schema;

namespace kit_stem_api.Models.Domain
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime ExpirationTime { get; set; }
    }
}
