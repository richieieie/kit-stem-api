using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO.Request
{
    public class LabSupportGetDTO
    {
        [Range(0, int.MaxValue, ErrorMessage = "Số trang phải lớn hơn hoặc bằng 0")]
        public int Page { get; set; } = 0;
        public bool Supported { get; set; } = false;
        public string LabSupportId { get; set; } = "";
        public string CustomerEmail { get; set; } = "";
        public bool OrderByCreateatDesc { get; set; } = true;
    }
}
