using Microsoft.AspNetCore.Mvc;

namespace KSH.Api.Models.DTO.Request
{
    public class ComponentGetDTO
    {
        public int Page { get; set; } = 0;
        public string? Name { get; set; }
    }
}
