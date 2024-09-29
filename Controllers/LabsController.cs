using kit_stem_api.Constants;
using kit_stem_api.Models.DTO;
using kit_stem_api.Services;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Controllers
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
                return StatusCode(StatusCodes.Status400BadRequest, new { status = serviceResponse.Status, details = serviceResponse.Details });
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }


        [HttpPost]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> CreateAsync([FromForm] LabUploadDTO labUploadDTO)
        {
            var labId = Guid.NewGuid();
            var serviceResponse = await _firebaseService.UploadFileAsync(FirebaseConstants.BucketPrivate, FirebaseConstants.LabsFolder, labId.ToString(), labUploadDTO.File!);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var url = serviceResponse.Details!["url"].ToString();
            serviceResponse = await _labService.CreateAsync(labUploadDTO, labId, url!);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = serviceResponse.Status, details = serviceResponse.Details });
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
                    return StatusCode(StatusCodes.Status500InternalServerError, new { status = serviceResponse.Status, details = serviceResponse.Details });
                }
                url = serviceResponse.Details!["url"].ToString();
            }

            serviceResponse = await _labService.UpdateAsync(labUpdateDTO, url);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { status = serviceResponse.Status, details = serviceResponse.Details });
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
                return StatusCode(StatusCodes.Status400BadRequest, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return NoContent();
        }
    }
}