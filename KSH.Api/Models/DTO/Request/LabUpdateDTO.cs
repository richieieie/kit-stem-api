using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KSH.Api.Models.DTO
{
    public class LabUpdateDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public int LevelId { get; set; }
        [Required]
        public int KitId { get; set; }
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        [Required]
        public long Price { get; set; }
        [Required]
        public int MaxSupportTimes { get; set; }
        [Required]
        [StringLength(100)]
        public string? Author { get; set; }
        [Required]
        public IFormFile? File { get; set; }
    }
}