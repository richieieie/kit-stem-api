using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO.Request
{
    public class LabSupportUpdateStaffDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid OrderSupportId { get; set; }
    }
}
