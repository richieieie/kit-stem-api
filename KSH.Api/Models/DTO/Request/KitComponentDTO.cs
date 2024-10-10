using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO.Request
{
    public class KitComponentDTO
    {
        [Required]
        public int KitId { get; set; }
        [Required]
        public int ComponentId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int ComponentQuantity { get; set; }
    }
}
