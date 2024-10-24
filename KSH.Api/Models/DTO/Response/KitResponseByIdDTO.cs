using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Request;

namespace KSH.Api.Models.DTO.Response
{
    public class KitResponseByIdDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Brief { get; set; }
        public string? Description { get; set; }
        public long PurchaseCost { get; set; }
        public bool Status { get; set; }
        public virtual KitsCategory? KitsCategory { get; set; }
        public virtual ICollection<KitImageDTO>? KitImages { get; set; }
        public List<KitComponentInKitDTO> Components { get; set; } = new();
    }
}
