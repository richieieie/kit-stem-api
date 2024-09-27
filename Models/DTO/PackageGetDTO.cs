namespace kit_stem_api.Models.DTO
{
    public class PackageGetDTO
    {
        public int Page { get; set; } = 0;
        public int LevelId { get; set; } = 0;
        public int FromPrice { get; set; } = 0;
        public int? ToPrice { get; set; } = int.MaxValue;
        public string? KitName { get; set; }
        public string? CategoryName { get; set; }
        public bool Status { get; set; } = true;
    }
}
