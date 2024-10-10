using Microsoft.AspNetCore.Mvc;

namespace KST.Api.Models.DTO.Request
{
    public class CartDTO
    {
        public int PackageId { get; set; }
        public int PackageQuantity { get; set; }
    }
}
