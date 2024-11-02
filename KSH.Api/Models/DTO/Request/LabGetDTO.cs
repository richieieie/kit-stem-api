using Microsoft.AspNetCore.Mvc;

namespace KSH.Api.Models.DTO
{
    public class LabGetDTO
    {
        public int Page { get; set; } = 0;
        [FromQuery(Name = "lab-name")]
        public string? LabName { get; set; }
        [FromQuery(Name = "kit-name")]
        public string? KitName { get; set; }
        [FromQuery]
        public bool Status { get; set; } = true;
    }
}