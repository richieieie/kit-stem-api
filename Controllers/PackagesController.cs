using kit_stem_api.Models.DTO;
using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageService _packageService;
        public PackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PackageGetFilterDTO packageGetFilterDTO)
        {
            var serviceResponse = await _packageService.GetAsync(packageGetFilterDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("{id:int}")]
        [ActionName(nameof(GetByIdAsync))]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var serviceResponse = await _packageService.GetByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpDelete]
        [Route("{id:int}")]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> RemoveByIdAsync([FromRoute] int id)
        {
            var serviceResponse = await _packageService.GetByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return NoContent();
        }

        [HttpPost]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> CreateAsync([FromBody] PackageCreateDTO packageCreateDTO)
        {
            var (serviceResponse, id) = await _packageService.CreateAsync(packageCreateDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return CreatedAtAction(nameof(GetByIdAsync), new { id }, new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}