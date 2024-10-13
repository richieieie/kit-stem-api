using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class KitImageDTO
    {
        public Guid Id { get; set; }
        public string? Url { get; set; }
    }
}
