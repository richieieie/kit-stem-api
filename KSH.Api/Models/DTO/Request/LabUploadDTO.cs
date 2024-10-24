using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO
{
    public class LabUploadDTO
    {
        [Required]
        public int LevelId { get; set; }
        [Required]
        public int KitId { get; set; }
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        [Required]
        public long Price { get; set; }
        [Required]
        public int MaxSupportTimes { get; set; }
        [Required]
        [StringLength(100)]
        public string? Author { get; set; }
        [Required]
        public bool Status { get; set; } = true;
        [Required]
        public IFormFile? File { get; set; }
    }
}