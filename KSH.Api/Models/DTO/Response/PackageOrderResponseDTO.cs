namespace KST.Api.Models.DTO.Response
{
    public class PackageOrderResponseDTO
    {
        public int PackageQuantity { get; set; }
        public virtual PackageResponseDTO Package { get; set; } = null!;
    }
}