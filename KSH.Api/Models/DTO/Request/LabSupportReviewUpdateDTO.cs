using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO.Request
{
    public class LabSupportReviewUpdateDTO
    {
        [Required(ErrorMessage = "Vui lòng cung cấp ID của đánh giá!")]
        public Guid Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Xếp hạng phải lớn hơn hoặc bằng 1")]
        public int Rating { get; set; }
        [Required(ErrorMessage = "Vui lòng thêm đánh giá")]
        public string FeedBack { get; set; } = "không có đánh giá";
    }
}
