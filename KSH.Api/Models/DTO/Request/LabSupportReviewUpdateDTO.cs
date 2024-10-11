using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO.Request
{
    public class LabSupportReviewUpdateDTO
    {
        [Required]
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string? FeedBack { get; set; }
    }
}
