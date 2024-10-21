using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO.Request
{
    public class LabSupportGetDTO
    {
        public int Page { get; set; }
        public bool Supported { get; set; } = false;
        public string LabSupportId { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
