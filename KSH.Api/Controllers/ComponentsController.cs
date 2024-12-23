﻿using KSH.Api.Models.DTO;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Services;
using KSH.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KSH.Api.Controllers
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
        public async Task<IActionResult> GetAsync([FromQuery] ComponentGetDTO componentGetDTO)
        {
            var serviceResponse = await _componentService.GetAsync(componentGetDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
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
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
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
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
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
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
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
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
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
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}
