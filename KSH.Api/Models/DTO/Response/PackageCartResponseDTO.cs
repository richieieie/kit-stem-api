namespace KSH.Api.Models.DTO.Response
{
    public class PackageCartResponseDTO
    {
        public int PackageId { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public virtual KitInPackageResponseDTO? Kit { get; set; }
        public virtual ICollection<LabInPackageResponseDTO>? Labs { get; set; }

    }
}
