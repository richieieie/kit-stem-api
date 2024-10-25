using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KSH.Api.Models.DTO.Request
{
    public class KitGetDTO
    {
        public int Page { get; set; } = 0;
        public string KitName { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public int LevelId { get; set; } = 0;
        public int FromPrice { get; set; } = 0;
        public int ToPrice { get; set; } = int.MaxValue;
    }
}
