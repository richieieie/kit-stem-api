using kit_stem_api.Models.DTO;
using kit_stem_api.Services;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentsController : ControllerBase
    {
        private readonly IComponentService _componentService;

        public ComponentsController(IComponentService componentService)
        {
            _componentService = componentService;
        }

        [HttpGet]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetAllAsync()
        {
            var serviceResponse = await _componentService.GetAllAsync();
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var serviceResponse = await _componentService.GetByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> CreateAsync(ComponentCreateDTO component)
        {
            var serviceResponse = await _componentService.CreateAsync(component);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> UpdateAsync(ComponentUpdateDTO component)
        {
            var serviceResponse = await _componentService.UpdateAsync(component);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> RemoveByIdAsync([FromRoute] int id)
        {
            var serviceResponse = await _componentService.RemoveByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return NoContent();
        }

        [HttpPut]
        [Route("Restore/{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> RestoreByIdAsync([FromRoute] int id)
        {
            var serviceResponse = await _componentService.RestoreByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}
