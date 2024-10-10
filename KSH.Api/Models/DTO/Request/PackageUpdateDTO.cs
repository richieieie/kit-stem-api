using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO.Request
{
    public class PackageUpdateDTO
    {
        [Required(ErrorMessage = "Vui lòng cung cấp Id của gói muốn thay đổi!")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp tên của gói!")]
        public string? Name { get; set; }
        public int LevelId { get; set; }
        public int Price { get; set; }
    }
}