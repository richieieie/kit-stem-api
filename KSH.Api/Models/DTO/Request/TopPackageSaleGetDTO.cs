using KSH.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class TopPackageSaleGetDTO
    {
        [Required]
        public DateTimeOffset FromDate {  get; set; }
        [Required]
        public DateTimeOffset ToDate { get; set; }
        public string ShippingStatus { get; set; } = OrderFulfillmentConstants.OrderSuccessStatus;
        public int PackageTop { get; set; } = 5;
        public bool BySale { get; set; } = true;
    }
}
