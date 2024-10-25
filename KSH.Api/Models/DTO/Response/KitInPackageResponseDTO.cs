using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Request;

namespace KSH.Api.Models.DTO.Response
{
    public class KitInPackageResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Brief { get; set; } = null!;
        public string Description { get; set; } = null!;
        public long PurchaseCost { get; set; }
        public bool Status { get; set; }
        public virtual KitsCategory? Category { get; set; }
        public virtual ICollection<KitImageDTO>? KitImages { get; set; }
    }
}