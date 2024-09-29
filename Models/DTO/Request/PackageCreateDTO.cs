using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO.Request
{
    public class PackageCreateDTO
    {
        [Required(ErrorMessage = "Vui lòng cung cấp tên của gói!")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp bộ kit của gói!")]
        public int KitId { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp cấp độ của gói!")]
        public int LevelId { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp giá của gói!")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Vui lòng cung trạng thái của gói!")]
        public bool Status { get; set; }
        public ICollection<Guid>? LabIds { get; set; }
    }
}