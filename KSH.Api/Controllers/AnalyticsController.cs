using KSH.Api.Models.DTO.Request;
using KSH.Api.Services;
using KSH.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetOrdersAnalyticsAsync([FromQuery] AnalyticOrderDTO analyticOrderDTO)
        {
            var serviceResponse = await _analyticService.GetOrderData(analyticOrderDTO);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("Packages/Top/{top:int}/Year/{year:int}")]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetOrdersAnalyticsAsync(int top, int year)
        {
            var serviceResponse = await _analyticService.GetTopPackageByYear(top, year);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Authorize(Roles = "manager")]
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
        [Authorize(Roles = "manager")]
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
        [HttpGet]
        [Authorize(Roles = "manager")]
        [Route("Package/Sale")]
        public async Task<IActionResult> GetTopPackageSaleAsync([FromQuery] TopPackageSaleGetDTO packageSaleGetDTO)
        {
            var serviceResponse = await _analyticService.GetTopPackageSale(packageSaleGetDTO);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Authorize(Roles = "manager")]
        [Route("Revenues/{year:int}")]
        public async Task<IActionResult> GetRevenuePerYear(int year)
        {
            var serviceResponse = await _analyticService.GetRevenuePerYear(year);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Authorize(Roles = "manager")]
        [Route("Profits/{year:int}")]
        public async Task<IActionResult> GetProfitPerYear(int year)
        {
            var serviceResponse = await _analyticService.GetProfitPerYear(year);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}