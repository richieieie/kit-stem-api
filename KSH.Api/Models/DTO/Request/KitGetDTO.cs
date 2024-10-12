using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KSH.Api.Models.DTO.Request
{
    public class KitGetDTO
    {
        public int Page { get; set; } = 0;
        [FromQuery(Name = "kit-name")]
        public string KitName { get; set; } = "";
        [FromQuery(Name = "category-name")]
        public string CategoryName { get; set; } = "";
        [FromQuery(Name = "min-price")]
        public int MinPrice { get; set; } = 0;
        [FromQuery(Name = "max-price")]
        public int MaxPrice { get; set; } = int.MaxValue;
    }
}
