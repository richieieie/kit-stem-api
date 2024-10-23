using KSH.Api.Services;
using KSH.Api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

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
        [Route("Revenues")]
        public async Task<IActionResult> GetRevenueAsync([FromQuery] DateTimeOffset fromDate, [FromQuery] DateTimeOffset toDate)
        {
            ServiceResponse serviceResponse = await _analyticService.GetRevenue(fromDate, toDate);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("Profits")]
        public async Task<IActionResult> GetProfitAsync([FromQuery] DateTimeOffset fromDate, [FromQuery] DateTimeOffset toDate)
        {
            ServiceResponse serviceResponse = await _analyticService.GetProfit(fromDate, toDate);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}