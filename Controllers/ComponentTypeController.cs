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
    public class ComponentTypeController : ControllerBase
    {
        private readonly IComponentTypeService _componentTypeService;

        public ComponentTypeController(IComponentTypeService componentTypeService)
        {
            _componentTypeService = componentTypeService;
        }

        [HttpGet("GetComponentTypes")]
        public async Task<IActionResult> GetComponentTypes()
        {
            var serviceResponse = await _componentTypeService.GetComponentTypes();
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost("CreateComponentType")]
        public async Task<IActionResult> CreateComponentType(ComponentTypeCreateDTO componentTypeCreateDTO)
        {
            var serviceResponse = await _componentTypeService.CreateComponentTypeAsync(componentTypeCreateDTO);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut("UpdateComponentType")]
        public async Task<IActionResult> UpdateComponentType(int Id, ComponentTypeUpdateDTO componentTypeUpdateDTO)
        {
            var serviceResponse = await _componentTypeService.UpdateComponentTypeAsync(Id, componentTypeUpdateDTO);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpDelete("DeleteComponentType")]
        public async Task<IActionResult> DeleteComponentType(int Id)
        {
            var serviceResponse = await _componentTypeService.DeleteComponentTypeAsync(Id);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}
