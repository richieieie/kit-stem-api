﻿using kit_stem_api.Models.DTO;
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
        public async Task<IActionResult> GetComponents()
        {
            var serviceResponse = await _componentService.GetAllAsync();
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> CreateComponent(ComponentCreateDTO component)
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
        public async Task<IActionResult> UpdateComponent(ComponentUpdateDTO component)
        {
            var serviceResponse = await _componentService.UpdateAsync(component);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpDelete]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> DeleteComponent(int id)
        {
            var serviceResponse = await _componentService.RemoveAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}