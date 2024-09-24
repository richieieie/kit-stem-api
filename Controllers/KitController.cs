using kit_stem_api.Models.DTO;
using kit_stem_api.Services;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KitController : ControllerBase
    {
        private readonly IKitService _kitService;

        public KitController(IKitService kitService)
        {
            _kitService = kitService;
        }
        [HttpGet]
        public async Task<IActionResult> GetKit()
        {
            var serviceResponse = await _kitService.GetAsync();
            if (!serviceResponse.Succeeded)
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
        [HttpPost]
        public async Task<IActionResult> CreateKit([FromForm]KitCreateDTO DTO)
        {
            var serviceResponse = await _kitService.CreateAsync(DTO);
            if (!serviceResponse.Succeeded)
                return BadRequest(new {status = serviceResponse.Status, detail = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }
        [HttpPut]
        public async Task<IActionResult> UpdateKit([FromForm]KitUpdateDTO DTO)
        {
            var serviceResponse = await _kitService.UpdateAsync(DTO);
            if (!serviceResponse.Succeeded)
                return BadRequest(new { status = serviceResponse.Status, detail = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }
        [HttpDelete]
        public async Task<IActionResult> UpdateKit([FromForm] int id)
        {
            var serviceResponse = await _kitService.DeleteAsync(id);
            if (!serviceResponse.Succeeded)
                return BadRequest(new { status = serviceResponse.Status, detail = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }
    }
}
