using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO.Request
{
    public class PackageUpdateDTO
    {
        [Required(ErrorMessage = "Vui lòng cung cấp Id của gói muốn thay đổi!")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp tên của gói!")]
        public string? Name { get; set; }
        public int LevelId { get; set; }
        public int Price { get; set; }
        public bool Status { get; set; } = true;
    }
}