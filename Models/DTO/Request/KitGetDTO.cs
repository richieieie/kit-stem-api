namespace kit_stem_api.Models.DTO.Request
{
    public class KitGetDTO
    {
        public int Page { get; set; } = 0;
        public string? KitName { get; set; }
        public string? CategoryName { get; set; }
    }
}
