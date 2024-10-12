namespace KSH.Api.Models.DTO.Request
{
    public class PackageOrderCreateDTO
    {
        public int PackageId { get; set; }

        public Guid OrderId { get; set; }

        public int PackageQuantity { get; set; }
    }
}
