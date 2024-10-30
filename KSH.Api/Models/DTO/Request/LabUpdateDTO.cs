using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO
{
    public class LabUpdateDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public int LevelId { get; set; }
        [Required]
        public int KitId { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp tên Lab", AllowEmptyStrings = false)]
        [StringLength(100)]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp giá lab từ 0 VND trở lên!")]
        [Range(0, long.MaxValue, ErrorMessage = "Vui lòng cung cấp giá lab từ 0 VND trở lên!")]
        public long Price { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp số lần hỗ trợ tối đa từ 0 trở lên!")]
        [Range(0, int.MaxValue, ErrorMessage = "Vui lòng cung cấp số lần hỗ trợ tối đa từ 0 trở lên!")]
        public int MaxSupportTimes { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp tên tác giả của bài lab!", AllowEmptyStrings = false)]
        [StringLength(100)]
        public string? Author { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp trạng thái.")]
        public bool Status { get; set; }
        public IFormFile? File { get; set; }
    }
}