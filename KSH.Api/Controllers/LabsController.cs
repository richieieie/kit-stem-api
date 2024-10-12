using System.Security.Claims;
using KSH.Api.Constants;
using KSH.Api.Models.DTO;
using KSH.Api.Services;
using KSH.Api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KSH.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabsController : ControllerBase
    {
        private readonly IFirebaseService _firebaseService;
        private readonly ILabService _labService;
        public LabsController(IFirebaseService firebaseService, ILabService labService)
        {
            _firebaseService = firebaseService;
            _labService = labService;
        }

        [HttpGet]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetAsync([FromQuery] LabGetDTO labGetDTO)
        {
            var serviceResponse = await _labService.GetAsync(labGetDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName(nameof(GetByIdAsync))]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var serviceResponse = await _labService.GetByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("{id:guid}/Download")]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> DownloadLabByIdAsync(Guid id)
        {
            var serviceResponse = await _labService.GetFileUrlByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var labUrl = serviceResponse.Details![ServiceResponse.ToKebabCase("url")].ToString();
            var labName = serviceResponse.Details![ServiceResponse.ToKebabCase("fileName")].ToString();
            serviceResponse = await _firebaseService.DownloadFileAsync(FirebaseConstants.BucketPrivate, labUrl!);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var stream = (MemoryStream)serviceResponse.Details![ServiceResponse.ToKebabCase("stream")];
            var contentType = serviceResponse.Details![ServiceResponse.ToKebabCase("contentType")].ToString();

            return File(stream, contentType!, labName);
        }

        [HttpGet]
        [Route("{id:guid}/Orders/{orderId:guid}/Download")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> DownloadLabByIdAsync(Guid id, Guid orderId)
        {
            var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var serviceResponse = await _labService.GetFileUrlByIdAndOrderIdAsync(customerId!, id, orderId);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var labUrl = serviceResponse.Details![ServiceResponse.ToKebabCase("url")].ToString();
            var labName = serviceResponse.Details![ServiceResponse.ToKebabCase("fileName")].ToString();
            serviceResponse = await _firebaseService.DownloadFileAsync(FirebaseConstants.BucketPrivate, labUrl!);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var stream = (MemoryStream)serviceResponse.Details![ServiceResponse.ToKebabCase("stream")];
            var contentType = serviceResponse.Details![ServiceResponse.ToKebabCase("contentType")].ToString();

            return File(stream, contentType!, labName);
        }


        [HttpPost]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> CreateAsync([FromForm] LabUploadDTO labUploadDTO)
        {
            var labId = Guid.NewGuid();
            var serviceResponse = await _firebaseService.UploadFileAsync(FirebaseConstants.BucketPrivate, FirebaseConstants.LabsFolder, labId.ToString(), labUploadDTO.File!);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var url = serviceResponse.Details![ServiceResponse.ToKebabCase("url")].ToString();
            serviceResponse = await _labService.CreateAsync(labUploadDTO, labId, url!);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return CreatedAtAction(nameof(GetByIdAsync), new { id = labId }, new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> UpdateAsync([FromForm] LabUpdateDTO labUpdateDTO)
        {
            ServiceResponse serviceResponse;
            string? url = null;
            if (labUpdateDTO.File != null)
            {
                serviceResponse = await _firebaseService.UploadFileAsync(FirebaseConstants.BucketPrivate, FirebaseConstants.LabsFolder, labUpdateDTO.Id.ToString(), labUpdateDTO.File!);
                if (!serviceResponse.Succeeded)
                {
                    return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
                }
                url = serviceResponse.Details![ServiceResponse.ToKebabCase("url")].ToString();
            }

            serviceResponse = await _labService.UpdateAsync(labUpdateDTO, url);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpDelete]
        [Route("{id:guid}")]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var serviceResponse = await _labService.RemoveByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return NoContent();
        }

        [HttpPut]
        [Route("Restore/{id:guid}")]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> RestoreAsync(Guid id)
        {
            var serviceResponse = await _labService.RestoreByIdAsync(id);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}