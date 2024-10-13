﻿using KST.Api.Models.DTO.Request;
using KST.Api.Services.IServices;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KST.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LabSupportsController : ControllerBase
    {
        private readonly ILabSupportService _labSupportService;
        public LabSupportsController(ILabSupportService labSupportService)
        {
            _labSupportService = labSupportService;
        }
        [HttpGet]
        // [Authorize(Roles = "staff")]
        public async Task<IActionResult> GetAsync([FromQuery]LabSupportGetDTO getDTO)
        {
            var serviceResponse = await _labSupportService.GetAsync(getDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, detail = serviceResponse.Details });
            }
            return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }
        [HttpPost]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult>CreateAsync(string orderId, int packageId, string labId)
        {
            var serviceResponse = await _labSupportService.CreateAsync(orderId, packageId, labId);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, detail = serviceResponse.Details});
            }
            return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }

        [HttpPut]
        [Route("{labSupportId:guid}/Accept")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> UpdateStaffIdAsync(Guid labSupportId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var serviceResponse = await _labSupportService.UpdateStaffAsync(userId!.ToString(), labSupportId);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
        [HttpPut]
        [Route("{labSupportId:guid}/Finished")]
        [Authorize(Roles = "staff")]
        public async Task<IActionResult> UpdateFinishedAsync(Guid labSupportId)
        {
            
            var serviceResponse = await _labSupportService.UpdateFinishedAsync(labSupportId);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
        [HttpPut]
        [Route("Review")]
        // [Authorize(Roles = "customer")]
        public async Task<IActionResult> UpdateReviewAsync(LabSupportReviewUpdateDTO DTO)
        {

            var serviceResponse = await _labSupportService.UpdateReviewAsync(DTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}
