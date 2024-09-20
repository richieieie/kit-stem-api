using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kit_stem_api.Constants;
using kit_stem_api.Models.DTO;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabController : ControllerBase
    {
        private readonly IFirebaseService _firebaseService;
        private readonly ILabService _labService;
        public LabController(IFirebaseService firebaseService, ILabService labService)
        {
            _firebaseService = firebaseService;
            _labService = labService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] LabUploadDTO labUploadDTO)
        {
            var serviceResponse = await _firebaseService.UploadFileAsync(FirebaseConstants.BucketPrivate, FirebaseConstants.LabsFolder, Guid.NewGuid().ToString(), labUploadDTO.File);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var url = serviceResponse.Details["url"].ToString();
            serviceResponse = await _labService.CreateAsync(labUploadDTO, url);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}