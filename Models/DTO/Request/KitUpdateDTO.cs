using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO.Request
{
    public class KitUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        public int CategoryId { get; set; } = 1;
        public string? Name { get; set; } = "";
        public string? Brief { get; set; } = "";
        public string? Description { get; set; } = "";
        public int PurchaseCost { get; set; } = 1;
        public List<IFormFile>? images { get; set; }
        public bool Status { get; set; } = true;
    }
}
