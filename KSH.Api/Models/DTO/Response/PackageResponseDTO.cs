using KST.Api.Models.Domain;

namespace KST.Api.Models.DTO.Response
{
    public class PackageResponseDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public bool Status { get; set; }
        public virtual Level? Level { get; set; }
        public virtual KitInPackageResponseDTO? Kit { get; set; }
        public virtual ICollection<LabInPackageResponseDTO>? PackageLabs { get; set; }
    }
}