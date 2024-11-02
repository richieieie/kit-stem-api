using System.ComponentModel.DataAnnotations.Schema;

namespace KSH.Api.Models.Domain
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public DateTimeOffset ExpirationTime { get; set; }
    }
}
