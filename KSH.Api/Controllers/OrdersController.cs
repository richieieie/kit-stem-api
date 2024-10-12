using System.Security.Claims;
using System.Security.Cryptography;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KSH.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        // [Authorize(Roles = "staff")]
        public async Task<IActionResult> GetAsync([FromQuery] OrderStaffGetDTO orderStaffGetDTO)
        {
            var serviceResponse = await _orderService.GetAsync(orderStaffGetDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("Customers")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> GetByCurrentCurrentCustomerAsync([FromQuery] OrderGetDTO orderGetDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var serviceResponse = await _orderService.GetByCustomerIdAsync(userId!, orderGetDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "staff,customer")]
        [ActionName(nameof(GetByIdAsync))]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var serviceResponse = await _orderService.GetByIdAsync(id, userId!, role!);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> CreateByCustomerIdAsync(bool isUsePoint, string shippingAddress, string note)
        {
            var userId = User.FindFirstValue(ClaimTypes.Email);
            var (serviceResponse, guid) = await _orderService.CreateByCustomerIdAsync(userId!, isUsePoint, shippingAddress, note);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return CreatedAtAction(nameof(GetByIdAsync), new { id = guid }, new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        // [HttpGet]
        // [Route("{id:guid}/OrderSupports")]
        // [Authorize(Roles = "staff,customer")]
        // public async Task<IActionResult> GetOrderSupportsByIdAsync()
        // {
        //     var serviceResponse = await _orderService.GetByIdAsync();
        //     if (!serviceResponse.Succeeded)
        //     {
        //         return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
        //     }

        //     return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        // }

        // [HttpGet]
        // [Route("OrderSupports/{id:guid}/LabSupports")]
        // public async Task<IActionResult> GetLabSupportByOrderSupportIdAsync()
        // {
        //     var serviceResponse = await _orderService.GetByIdAsync();
        //     if (!serviceResponse.Succeeded)
        //     {
        //         return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
        //     }

        //     return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        // }
    }
}