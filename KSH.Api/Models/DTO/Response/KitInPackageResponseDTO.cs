using KSH.Api.Models.Domain;

namespace KSH.Api.Models.DTO.Response
{
    public class KitInPackageResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Brief { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int PurchaseCost { get; set; }
        public bool Status { get; set; }
        public virtual KitsCategory? Category { get; set; }
        public virtual ICollection<KitImage>? KitImages { get; set; }
    }
}