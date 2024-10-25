using Microsoft.AspNetCore.Mvc;

namespace KSH.Api.Models.DTO
{
    public class PackageGetFilterDTO
    {
        public int Page { get; set; } = 0;
        public string? Name { get; set; }
        public int LevelId { get; set; } = 0;
        public long FromPrice { get; set; } = 0;
        public long ToPrice { get; set; } = long.MaxValue;
        public string? KitName { get; set; }
        public string? CategoryName { get; set; }
        public bool Status { get; set; } = true;
        public bool IncludeLabs { get; set; } = false;
    }
}
