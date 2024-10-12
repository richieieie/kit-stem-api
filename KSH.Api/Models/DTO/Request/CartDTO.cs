using Microsoft.AspNetCore.Mvc;

namespace KSH.Api.Models.DTO.Request
{
    public class CartDTO
    {
        public int PackageId { get; set; }
        public int PackageQuantity { get; set; }
    }
}
