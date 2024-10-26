using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KSH.Api.Models.DTO
{
    public class CategoryCreateDTO
    {
        [StringLength(100)]
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
    }
}
