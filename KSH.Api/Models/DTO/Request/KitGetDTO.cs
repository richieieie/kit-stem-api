using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class KitGetDTO
    {
        [Range(0, int.MaxValue, ErrorMessage = "Trang phải lớn hơn hoặc bằng 0")]
        public int Page { get; set; } = 0;
        public string KitName { get; set; } = "";
        public string CategoryName { get; set; } = "";
        [Range(0, int.MaxValue, ErrorMessage = "Cấp độ phải lớn hơn hoặc bằng 0")]
        public int LevelId { get; set; } = 0;
        [Range(0, int.MaxValue, ErrorMessage = "Giá từ phải lớn hơn hoặc bằng 0")]
        public int FromPrice { get; set; } = 0;
        [Range(0, int.MaxValue, ErrorMessage = "Giá đến phải lớn hơn hoặc bằng 0")]
        public int ToPrice { get; set; } = int.MaxValue;
        public bool Status { get; set; } = true;
    }
}
