namespace KSH.Api.Models.DTO.Request
{
    public class KitGetDTO
    {
        public int Page { get; set; } = 0;
        public string KitName { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public long FromPrice { get; set; } = 0;
        public long ToPrice { get; set; } = long.MaxValue;
    }
}
