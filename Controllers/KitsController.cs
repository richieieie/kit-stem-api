using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Services;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KitsController : ControllerBase
    {
        private readonly IKitService _kitService;

        public KitsController(IKitService kitService)
        {
            _kitService = kitService;
        }

        [HttpGet]
        //[Authorize(Roles = "manager")]
        public async Task<IActionResult> GetAsync([FromQuery] KitGetDTO kitGetDTO)
        {
            var serviceResponse = await _kitService.GetAsync(kitGetDTO);
            if (!serviceResponse.Succeeded)
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("{id:int}")]
        [ActionName(nameof(GetByIdAsync))]
        //[Authorize(Roles = "manager")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var serviceResponse = await _kitService.GetByIdAsync(id);
            if (!serviceResponse.Succeeded)
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("packages/{kitId:int}")]
        [ActionName(nameof(GetByKitIdAsync))]
        public async Task<IActionResult> GetByKitIdAsync(int kitId)
        {
            var serviceResponse = await _kitService.GetPackagesByKitId(kitId);
            if (!serviceResponse.Succeeded)
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        //[Authorize(Roles = "manager")]
        public async Task<IActionResult> CreateAsync([FromForm]KitCreateDTO DTO)
        {
            var serviceResponse = await _kitService.CreateAsync(DTO);
            if (!serviceResponse.Succeeded)
                return BadRequest(new {status = serviceResponse.Status, detail = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }

        [HttpPut]
        //[Authorize(Roles = "manager")]
        public async Task<IActionResult> UpdateAsync(KitUpdateDTO DTO)
        {
            var serviceResponse = await _kitService.UpdateAsync(DTO);
            if (!serviceResponse.Succeeded)
                return BadRequest(new { status = serviceResponse.Status, detail = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }

        [HttpDelete]
        [Route("{id:int}")]
        //[Authorize(Roles = "manager")]
        public async Task<IActionResult> RemoveByIdAsync([FromForm] int id)
        {
            var serviceResponse = await _kitService.RemoveAsync(id);
            if (!serviceResponse.Succeeded)
                return BadRequest(new { status = serviceResponse.Status, detail = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }

        [HttpPut]
        [Route("Restore/{id:int}")]
        //[Authorize(Roles = "manager")]
        public async Task<IActionResult> RestoreByIdAsync([FromForm] int id)
        {
            var serviceResponse = await _kitService.RestoreByIdAsync(id);
            if (!serviceResponse.Succeeded)
                return BadRequest(new { status = serviceResponse.Status, detail = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }
    }
}
