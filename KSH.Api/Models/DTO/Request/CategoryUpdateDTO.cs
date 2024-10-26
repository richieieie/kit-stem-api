using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO
{
    public class CategoryUpdateDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập ID!")]
        [Range(1, int.MaxValue, ErrorMessage = "ID phải lớn hơn hoặc bằng 1")]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
