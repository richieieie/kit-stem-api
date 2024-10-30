using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class PackageGetByKitIdDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập id")]
        public int KitId { get; set; }
        public bool Status { get; set; } = true;
    }
}
