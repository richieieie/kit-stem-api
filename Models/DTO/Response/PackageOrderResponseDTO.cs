using kit_stem_api.Models.Domain;

namespace kit_stem_api.Models.DTO.Response
{
    public class PackageOrderResponseDTO
    {
        public int PackageQuantity { get; set; }
        public virtual Package Package { get; set; } = null!;
    }
}