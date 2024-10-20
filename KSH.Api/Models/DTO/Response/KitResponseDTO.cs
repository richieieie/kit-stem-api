using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Request;

namespace KSH.Api.Models.DTO.Response
{
    public class KitResponseDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string Brief { get; set; } = null!;
        public string Description { get; set; } = null!;
        public long MinPackagePrice { get; set; }
        public long MaxPackagePrice { get; set; }

        public int PurchaseCost { get; set; }

        public bool Status { get; set; }
        public virtual KitsCategory? KitsCategory { get; set; }
        public virtual ICollection<KitImageDTO>? KitImages { get; set; }
    }
}
