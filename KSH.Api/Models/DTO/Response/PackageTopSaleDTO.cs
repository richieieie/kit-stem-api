namespace KSH.Api.Models.DTO.Response
{
    public class PackageTopSaleDTO
    {
        public int PackageId { get; set; }
        public decimal TotalPackagePrice { get; set; }
        public decimal TotalProfit { get; set; }
        public int KitId { get; set; }
        public string? KitName { get; set; }
    }
}
