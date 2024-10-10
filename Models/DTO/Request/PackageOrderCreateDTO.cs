namespace kit_stem_api.Models.DTO.Request
{
    public class PackageOrderCreateDTO
    {
        public int PackageId { get; set; }

        public Guid OrderId { get; set; }

        public int PackageQuantity { get; set; }
    }
}
