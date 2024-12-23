﻿using KSH.Api.Models.DTO;
using KSH.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KSH.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }


        [HttpGet]
         [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllAsync()
        {
            var serviceResponse = await _categoryService.GetAsync();
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var serviceResponse = await _categoryService.GetByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateAsync(CategoryCreateDTO categoryDTO)
        {
            var serviceResponse = await _categoryService.CreateAsync(categoryDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateAsync(CategoryUpdateDTO categoryUpdateDTO)
        {
            var serviceResponse = await _categoryService.UpdateAsync(categoryUpdateDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoveByIdAsync([FromRoute] int id)
        {
            var serviceResponse = await _categoryService.RemoveByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return NoContent();
        }

        [HttpPut]
        [Route("Restore/{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RestoreByIdAsync([FromRoute] int id)
        {
            var serviceResponse = await _categoryService.RestoreByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

    }
}


