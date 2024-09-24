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
    public class ComponentTypesController : ControllerBase
    {
        private readonly IComponentTypeService _componentTypeService;

        public ComponentTypesController(IComponentTypeService componentTypeService)
        {
            _componentTypeService = componentTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetComponentTypes()
        {
            var serviceResponse = await _componentTypeService.GetAllAsync();
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        public async Task<IActionResult> CreateComponentType(ComponentTypeCreateDTO componentTypeCreateDTO)
        {
            var serviceResponse = await _componentTypeService.CreateAsync(componentTypeCreateDTO);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateComponentType(ComponentTypeUpdateDTO componentTypeUpdateDTO)
        {
            var serviceResponse = await _componentTypeService.UpdateAsync(componentTypeUpdateDTO);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteComponentType(int Id)
        {
            var serviceResponse = await _componentTypeService.RemoveAsync(Id);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}
