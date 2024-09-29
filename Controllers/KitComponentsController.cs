using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Services;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitComponentsController : ControllerBase
    {
        private readonly IKitComponentService _kitComponentService;

        public KitComponentsController(IKitComponentService kitComponentService)
        {
            _kitComponentService = kitComponentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var serviceResponse = await _kitComponentService.GetAllAsync();
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("{kitId:int}")]
        public async Task<IActionResult> GetByKitIdAsync([FromRoute]int kitId) 
        {
            var serviceResponse = await _kitComponentService.GetByKitIdAsync(kitId);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(KitComponentDTO kitComponentDTO)
        {
            var serviceResponse = await _kitComponentService.CreateAsync(kitComponentDTO);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(KitComponentDTO kitComponentDTO)
        {
            var serviceResponse = await _kitComponentService.UpdateAsync(kitComponentDTO);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}
