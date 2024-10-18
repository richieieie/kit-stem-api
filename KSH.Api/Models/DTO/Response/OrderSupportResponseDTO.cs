namespace KSH.Api.Models.DTO.Response
{
    public class OrderSupportResponseDTO
    {
        public int RemainSupportTimes { get; set; }
        public LabResponseDTO? Lab { get; set; }
        public PackageResponseDTO? Package { get; set; }
    }
}