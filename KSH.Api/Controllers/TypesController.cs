using KST.Api.Models.DTO;
using KST.Api.Services;
using KST.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KST.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private readonly IComponentTypeService _componentTypeService;

        public TypesController(IComponentTypeService componentTypeService)
        {
            _componentTypeService = componentTypeService;
        }

        [HttpGet]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllAsync()
        {
            var serviceResponse = await _componentTypeService.GetAllAsync();
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("{id:int}")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var serviceResponse = await _componentTypeService.GetByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateAsync(ComponentTypeCreateDTO componentTypeCreateDTO)
        {
            var serviceResponse = await _componentTypeService.CreateAsync(componentTypeCreateDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAsync(ComponentTypeUpdateDTO componentTypeUpdateDTO)
        {
            var serviceResponse = await _componentTypeService.UpdateAsync(componentTypeUpdateDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpDelete]
        [Route("{id:int}")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoveByIdAsync([FromRoute] int id)
        {
            var serviceResponse = await _componentTypeService.RemoveByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return NoContent();
        }

        [HttpPut]
        [Route("Restore/{id:int}")]
        // [Authorize(Roles = "admin")]
        public async Task<IActionResult> RestoreByIdAsync([FromRoute] int id)
        {
            var serviceResponse = await _componentTypeService.RestoreByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}
