using KST.Api.Models.DTO.Request;
using KST.Api.Services;
using KST.Api.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KST.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderSupportsController : ControllerBase
    {
        private readonly IOrderSupportService _orderSupportService;
        public OrderSupportsController(IOrderSupportService orderSupportService)
        {
            _orderSupportService = orderSupportService;
        }
        [HttpGet]
        public async Task<IActionResult>GetAsync([FromQuery]OrderSupportGetDTO getDTO)
        {
            var serviceResponse = await _orderSupportService.GetAsync(getDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, detail = serviceResponse.Details });
            }
            return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }
    }
}
