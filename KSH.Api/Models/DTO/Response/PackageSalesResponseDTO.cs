namespace KSH.Api.Models.DTO.Response
{
    public class PackageSalesResponseDTO
    {
        public int? PackageId { get; set; }
        public string? PackageName { get; set; }
        public int KitId { get; set; }
        public string? KitName { get; set; }
        public int SoldQuantity { get; set; }
    }
}