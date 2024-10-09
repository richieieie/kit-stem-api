namespace kit_stem_api.Models.DTO.Response
{
    public class CartResponseDTO
    {
        public int PackageId { get; set; }
        public int PackageQuantity { get; set; }
        public PackageCartResponseDTO Package { get; set; }
    }
}
