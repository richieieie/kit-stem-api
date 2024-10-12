namespace KSH.Api.Models.DTO.Response
{
    public class CartResponseDTO
    {
        public int PackageQuantity { get; set; }
        public PackageCartResponseDTO? Package { get; set; }
    }
}
