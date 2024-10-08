using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO.Request;
using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO.Response
{
    public class KitResponseDTO
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string Brief { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int PurchaseCost { get; set; }

        public bool Status { get; set; }
        public virtual KitsCategory? KitsCategory { get; set; }

        public virtual ICollection<KitImageDTO> KitImages { get; set; }
    }
}
