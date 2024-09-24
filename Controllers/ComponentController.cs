using kit_stem_api.Models.DTO;
using kit_stem_api.Services;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComponentController : ControllerBase
    {
        private readonly IComponentService _componentService;

        public ComponentController(IComponentService componentService)
        {
            _componentService = componentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetComponents()
        {
            var serviceResponse = await _componentService.GetComponentsAsync();
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        public async Task<IActionResult> CreateComponent(ComponentCreateDTO component)
        {
            var serviceResponse = await _componentService.CreateComponentAsync(component);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComponent(int Id, ComponentUpdateDTO component)
        {
            var serviceResponse = await _componentService.UpdateComponentAsync(Id, component);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteComponent(int Id)
        {
            var serviceResponse = await _componentService.DeleteComponentAsync(Id);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}
