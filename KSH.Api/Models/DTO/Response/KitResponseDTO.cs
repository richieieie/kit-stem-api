using KST.Api.Models.Domain;
using KST.Api.Models.DTO.Request;
using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO.Response
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
