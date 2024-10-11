using KST.Api.Models.DTO.Request;
using KST.Api.Services.IServices;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KST.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabSupportController : ControllerBase
    {
        private readonly ILabSupportService _labSupportService;
        public LabSupportController(ILabSupportService labSupportService)
        {
            _labSupportService = labSupportService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery]LabSupportGetDTO getDTO)
        {
            var serviceResponse = await _labSupportService.GetAsync(getDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, detail = serviceResponse.Details });
            }
            return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }

        [HttpPost]
        [Route("{orderId:guid}")]
        public async Task<IActionResult>CreateAsync(Guid orderId)
        {
            var serviceResponse = await _labSupportService.CreateAsync(orderId);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, detail = serviceResponse.Details});
            }
            return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }
        [HttpPut]
        [Route("{labSupportId:guid}")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> UpdateAsync(Guid labSupportId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var serviceResponse = await _labSupportService.UpdateStaffAsync(userId.ToString(), labSupportId);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}
