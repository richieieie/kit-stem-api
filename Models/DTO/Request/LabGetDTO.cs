using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kit_stem_api.Models.DTO
{
    public class LabGetDTO
    {
        public int Page { get; set; } = 0;
        public string? LabName { get; set; }
        public string? KitName { get; set; }
    }
}