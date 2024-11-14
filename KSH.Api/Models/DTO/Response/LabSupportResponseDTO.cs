using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Response;

namespace KST.Api.Models.DTO.Response
{
    public class LabSupportResponseDTO
    {
        public Guid Id { get; set; }
        public Guid OrderSupportId { get; set; }
        public Guid LabId { get; set; }
        public string? StaffId { get; set; }
        public int Rating { get; set; }
        public string? FeedBack { get; set; }
        public bool IsFinished { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string? OrderPhoneNumber { get; set; }
        public virtual UserStaffInLabSupportDTO? Staff { get; set; }
        public virtual UserInLabSupportDTO? User { get; set; }
        public virtual LabInLabSupportResponseDTO? Lab { get; set; }
        public virtual Package? Package { get; set; }

    }
}
