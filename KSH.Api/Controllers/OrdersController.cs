using System.Security.Claims;
using System.Security.Cryptography;
using KSH.Api.Constants;
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
        private readonly IMapboxService _mapboxService;
        public OrdersController(IOrderService orderService, IMapboxService mapboxService)
        {
            _mapboxService = mapboxService;
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "staff")]
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
        public async Task<IActionResult> GetByCurrentCustomerAsync([FromQuery] OrderGetDTO orderGetDTO)
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
        // [Authorize(Roles = "staff,customer")]
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
        public async Task<IActionResult> CreateByCustomerIdAsync(bool isUsePoint, string shippingAddress, string phoneNumber, string? note)
        {
            var userId = User.FindFirstValue(ClaimTypes.Email);
            var (serviceResponse, guid) = await _orderService.CreateByCustomerIdAsync(userId!, isUsePoint, shippingAddress, phoneNumber, note ?? "");
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return CreatedAtAction(nameof(GetByIdAsync), new { id = guid }, new { status = serviceResponse.Status, details = serviceResponse.Details });
        }


        [HttpGet]
        [Route("GetDistanceTest")]
        public async Task<IActionResult> GetDistance([FromQuery] string address)
        {
            var serviceResponse = await _mapboxService.GetDistanceBetweenAnAddressAndShop(address);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut]
        [Route("{orderId:guid}/verified")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> UpdateVerifiedStatus(Guid orderId)
        {
            OrderShippingStatusUpdateDTO getDTO = new() { Id = orderId, ShippingStatus = OrderFulfillmentConstants.OrderVerifiedStatus };
            var serviceResponse = await _orderService.UpdateShippingStatus(getDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
        [HttpPut]
        [Route("{orderId:guid}/delivering")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> UpdateDeliveringStatus(Guid orderId)
        {
            OrderShippingStatusUpdateDTO getDTO = new() { Id = orderId, ShippingStatus = OrderFulfillmentConstants.OrderDeliveringStatus };
            var serviceResponse = await _orderService.UpdateShippingStatus(getDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
        [HttpPut]
        [Route("{orderId:guid}/success")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> UpdateSuccessStatus(Guid orderId)
        {
            OrderShippingStatusUpdateDTO getDTO = new() { Id = orderId, ShippingStatus = OrderFulfillmentConstants.OrderSuccessStatus };
            var serviceResponse = await _orderService.UpdateShippingStatus(getDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
        [HttpPut]
        [Route("{orderId:guid}/fail")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> UpdateFailStatus(Guid orderId)
        {
            OrderShippingStatusUpdateDTO getDTO = new() { Id = orderId, ShippingStatus = OrderFulfillmentConstants.OrderFailStatus };
            var serviceResponse = await _orderService.UpdateShippingStatus(getDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
        [HttpPut]
        [Route("{orderId:guid}/cancel")]
        // [Authorize(Roles = "customer")]
        public async Task<IActionResult> UpdateCancelStatus(Guid orderId)
        {
            OrderShippingStatusUpdateDTO getDTO = new() { Id = orderId, ShippingStatus = OrderFulfillmentConstants.OrderFailStatus };
            var serviceResponse = await _orderService.CancelShippingStatus(getDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("shippingfees")]
        public async Task<IActionResult> GetShippingFee([FromQuery] string address)
        {
            var serviceResponse = await _orderService.GetShippingFee(address);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        } 
    }
}