using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using kit_stem_api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.DTO
{
    public class LabUploadDTO
    {
        [Required]
        public int LevelId { get; set; }
        [Required]
        public int KitId { get; set; }
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int MaxSupportTimes { get; set; }
        [Required]
        [StringLength(100)]
        public string? Author { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public IFormFile? File { get; set; }
    }
}