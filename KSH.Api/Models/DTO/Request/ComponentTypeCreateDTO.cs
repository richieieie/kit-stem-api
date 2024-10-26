using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO
{
    public class ComponentTypeCreateDTO
    {
        [StringLength(100)]
        [Required(ErrorMessage = "Vui lòng nhập tên!")]
        public string Name { get; set; } = null!;
    }
}
