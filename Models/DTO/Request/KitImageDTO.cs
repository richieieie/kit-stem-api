using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO.Request
{
    public class KitImageDTO
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
    }
}
