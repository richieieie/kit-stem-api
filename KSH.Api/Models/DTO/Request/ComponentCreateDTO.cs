using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO
{
    public class ComponentCreateDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập loại linh kiện!")]
        [Range(1, int.MaxValue, ErrorMessage = "Loại linh kiện phải lớn hơn hoặc bằng 1")]
        public int TypeId { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        public string Name { get; set; } = null!;
    }
}
