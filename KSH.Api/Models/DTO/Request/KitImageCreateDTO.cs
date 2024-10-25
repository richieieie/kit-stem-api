using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class KitImageCreateDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập ID")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ID của kit")]
        public int KitId { get; set; }

        [Unicode(false)]
        public string? Url { get; set; }
    }
}
