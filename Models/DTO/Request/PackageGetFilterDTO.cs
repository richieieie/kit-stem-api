using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Models.DTO
{
    public class PackageGetFilterDTO
    {
        public int Page { get; set; } = 0;
        public string? Name { get; set; }
        [FromQuery(Name = "level-id")]
        public int LevelId { get; set; } = 0;
        [FromQuery(Name = "from-price")]
        public int FromPrice { get; set; } = 0;
        [FromQuery(Name = "to-price")]
        public int? ToPrice { get; set; } = int.MaxValue;
        [FromQuery(Name = "kit-name")]
        public string? KitName { get; set; }
        [FromQuery(Name = "category-name")]
        public string? CategoryName { get; set; }
        public bool Status { get; set; } = true;
        [FromQuery(Name = "include-labs")]
        public bool IncludeLabs { get; set; } = false;
    }
}
