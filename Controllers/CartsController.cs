using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace kit_stem_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> GetAsync()
        {
            var user = User.FindFirst(ClaimTypes.Email)?.Value;
            var serviceResponse = await _cartService.GetByIdAsync(user!);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> CreateAsync(CartDTO cartDTO)
        {
            var userId = User.FindFirst(ClaimTypes.Email)?.Value;

            var serviceResponse = await _cartService.CreateAsync(userId!, cartDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> UpdateAsync(CartDTO cartDTO)
        {
            var userId = User.FindFirst(ClaimTypes.Email)?.Value;

            var serviceResponse = await _cartService.UpdateAsync(userId!, cartDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpDelete]
        [Route("{packageId:int}")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> RemoveByPackageIdAsync(int packageId)
        {
            var userId = User.FindFirst(ClaimTypes.Email)?.Value;

            var serviceResponse = await _cartService.RemoveByPackageIdAsync(userId!, packageId);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> RemoveAllAsync()
        {
            var userId = User.FindFirst(ClaimTypes.Email)?.Value;

            var serviceResponse = await _cartService.RemoveAllAsync(userId!);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return NoContent();
        }
    }
}
