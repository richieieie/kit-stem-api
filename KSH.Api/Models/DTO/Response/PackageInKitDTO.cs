using KSH.Api.Models.Domain;

namespace KSH.Api.Models.DTO.Response
{
    public class PackageInKitDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public bool Status { get; set; }
        public virtual Level? Level { get; set; }
        public virtual KitResponseDTO? Kit { get; set; }
        public virtual ICollection<LabInPackageResponseDTO>? PackageLabs { get; set; }
    }
}
