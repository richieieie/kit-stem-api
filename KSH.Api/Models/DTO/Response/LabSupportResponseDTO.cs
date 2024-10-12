namespace KST.Api.Models.DTO.Response
{
    public class LabSupportResponseDTO
    {
        public Guid Id { get; set; }
        public Guid OrderSupportId { get; set; }
        public string? UserName { get; set; }
        public string? StaffId { get; set; }
        public int Rating { get; set; }
        public string? FeedBack { get; set; }
        public bool IsFinished { get; set; }
        public int RemainSupportTimes { get; set; }

    }
}
