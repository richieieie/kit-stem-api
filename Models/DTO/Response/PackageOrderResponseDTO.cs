namespace kit_stem_api.Models.DTO.Response
{
    public class PackageOrderResponseDTO
    {
        public int PackageQuantity { get; set; }
        public virtual PackageResponseDTO Package { get; set; } = null!;
    }
}