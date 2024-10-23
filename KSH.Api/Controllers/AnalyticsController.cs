using KSH.Api.Models.DTO.Request;
using KSH.Api.Services;
using KSH.Api.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Utilities.Date;
using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticService _analyticService;
        public AnalyticsController(IAnalyticService analyticService)
        {
            _analyticService = analyticService;
        }

        [HttpGet]
        [Route("Orders")]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetOrdersAnalyticsAsync([FromQuery] DateTimeOffset fromDate, [FromQuery] DateTimeOffset toDate, [FromQuery] string? shippingStatus)
        {
            ServiceResponse serviceResponse = await _analyticService.GetOrderData(fromDate, toDate, shippingStatus);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
        [HttpGet]
        [Route("Package/Sale")]
        public async Task<IActionResult> GetTopPackageSaleAsync([FromQuery]TopPackageSaleGetDTO packageSaleGetDTO)
        {
            ServiceResponse serviceResponse = await _analyticService.GetTopPackageSale(packageSaleGetDTO);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details});
        }
        [HttpGet]
        [Route("Package/Profit")]
        public async Task<IActionResult> GetTopPackageProfitAsync([FromQuery] TopPackageSaleGetDTO packageSaleGetDTO)
        {
            ServiceResponse serviceResponse = await _analyticService.GetTopPackageProfit(packageSaleGetDTO);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}