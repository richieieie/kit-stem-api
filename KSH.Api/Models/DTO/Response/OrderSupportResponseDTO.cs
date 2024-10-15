namespace KSH.Api.Models.DTO.Response
{
    public class OrderSupportResponseDTO
    {
        public Guid LabId { get; set; }
        public Guid OrderId { get; set; }
        public int PackageId { get; set; }
        public int RemainSupportTimes { get; set; }
        public LabResponseDTO? Lab { get; set; }
    }
}