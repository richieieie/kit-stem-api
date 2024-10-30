using KSH.Api.Constants;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Services;
using KSH.Api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace KSH.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KitsController : ControllerBase
    {
        private readonly IKitService _kitService;
        private readonly IKitImageService _kitImageService;
        private readonly IFirebaseService _firebaseService;

        public KitsController(IKitService kitService, IKitImageService kitImageService, IFirebaseService firebaseService)
        {
            _kitService = kitService;
            _kitImageService = kitImageService;
            _firebaseService = firebaseService;
        }
        #region Controller methods
        [HttpGet]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetAsync([FromQuery] KitGetDTO kitGetDTO)
        {
            var serviceResponse = await _kitService.GetAsync(kitGetDTO);
            if (!serviceResponse.Succeeded)
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("{id:int}")]
        [ActionName(nameof(GetByIdAsync))]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var serviceResponse = await _kitService.GetByIdAsync(id);
            if (!serviceResponse.Succeeded)
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("{kitId:int}/Packages")]
        [ActionName(nameof(GetPackagesByKitIdAsync))]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetPackagesByKitIdAsync(int kitId, bool packageStatus = true)
        {
            PackageGetByKitIdDTO DTO = new() { KitId = kitId, Status = packageStatus };
            var serviceResponse = await _kitService.GetPackagesByKitId(DTO);
            if (!serviceResponse.Succeeded)
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("{kitId:int}/Lab")]
        [ActionName(nameof(GetLabByKitIdAsync))]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetLabByKitIdAsync(int kitId)
        {
            var serviceResponse = await _kitService.GetLabByKitId(kitId);
            if (!serviceResponse.Succeeded)
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
        [HttpPost]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> CreateAsync([FromForm] KitCreateDTO DTO)
        {
            var serviceResponse = await _kitService.CreateAsync(DTO);

            if (!serviceResponse.Succeeded)
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            if (DTO.KitImagesList == null || DTO.KitImagesList!.Count == 0)
            {
                return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
            }
            // phần upload ảnh
            var kitId = await _kitService.GetMaxIdAsync(); // lấy kitId cuối cùng
            var kitIdString = kitId.ToString(); // đổi tử int to string
            int kitImageCount = 1; // dùng để đếm số image gửi xuống và đồng thời dùng để đặt tên cho file name image
            var nameFiles = new Dictionary<string, IFormFile>();
            List<Guid> imageGuidList = new List<Guid>();
            foreach (var image in DTO.KitImagesList!)
            {
                var imageIdTemp = Guid.NewGuid();
                imageGuidList.Add(imageIdTemp);
                nameFiles.Add(imageIdTemp.ToString(), image);
                kitImageCount++;
            }
            var filesServiceResponse = await _firebaseService.UploadFilesAsync
                    (FirebaseConstants.BucketPublic,
                     FirebaseConstants.ImagesKitsFolder + $"/{kitId}",
                     nameFiles); // image lên cloude 

            if (!filesServiceResponse.Succeeded) return StatusCode(filesServiceResponse.StatusCode, new { status = filesServiceResponse.Status, details = filesServiceResponse.Details });
            // -----------------//
            List<String>? urls = filesServiceResponse.Details![ServiceResponse.ToKebabCase("urls")] as List<String>;
            Guid imageId = Guid.Empty;
            if (urls != null)
            {
                for (int i = 0; i < (kitImageCount - 1); i++)
                {
                    var url = "";
                    url = urls.ElementAt(i);
                    foreach (var imageGuid in imageGuidList)
                    {
                        if (url.Contains(imageGuid.ToString()))
                        {
                            imageId = imageGuid;
                        }
                    }

                    var imageServiceResponse = await _kitImageService.CreateAsync(imageId, kitId, url);
                    if (!imageServiceResponse.Succeeded) StatusCode(imageServiceResponse.StatusCode, new { status = imageServiceResponse.Status, details = imageServiceResponse.Details });
                }
            }
            return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }
        [HttpPut]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> UpdateAsync([FromForm] KitUpdateDTO DTO)
        {
            ServiceResponse serviceResponse = null;
            if (DTO.KitImagesList == null)
            {
                serviceResponse = await _kitService.UpdateAsync(DTO);
                if (!serviceResponse.Succeeded)
                    return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, detail = serviceResponse.Details });

                return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
            }
            #region method hander fileBase
            var imageServiceResponse = await _kitImageService.RemoveAsync(DTO.Id);
            if (!imageServiceResponse.Succeeded) return StatusCode(imageServiceResponse.StatusCode, new { status = imageServiceResponse.Status, details = imageServiceResponse.Details });

            if (DTO.KitImagesList != null)
            {
                int kitImageCount = 1;
                var nameFiles = new Dictionary<string, IFormFile>();
                var imageIdList = new List<Guid>();

                foreach (var image in DTO.KitImagesList)
                {
                    Guid imageIdTemp = Guid.NewGuid();
                    imageIdList.Add(imageIdTemp);
                    nameFiles.Add(imageIdTemp.ToString(), image);
                    kitImageCount++;
                }

                var fileServiceResponse = await _firebaseService.UploadFilesAsync
                (FirebaseConstants.BucketPublic,
                     FirebaseConstants.ImagesKitsFolder + $"/{DTO.Id}",
                     nameFiles); // image lên cloude 
                if (!fileServiceResponse.Succeeded) return StatusCode(fileServiceResponse.StatusCode, new { status = fileServiceResponse.Status, details = fileServiceResponse.Details });
                List<String>? urls = fileServiceResponse.Details![ServiceResponse.ToKebabCase("urls")] as List<String>;
                Guid imageId = Guid.Empty;

                if (urls != null)
                {
                    for (int i = 0; i < (kitImageCount - 1); i++)
                    {
                        var url = urls.ElementAt(i);
                        foreach (var GuidId in imageIdList)
                        {
                            if (url.Contains(GuidId.ToString())) imageId = GuidId;
                        }
                        imageServiceResponse = await _kitImageService.CreateAsync(imageId, DTO.Id, url);
                        if (!imageServiceResponse.Succeeded) return StatusCode(imageServiceResponse.StatusCode, new { status = imageServiceResponse.Status, details = imageServiceResponse.Details });
                    }
                }

            }
            else
            {
                var fileServiceResponse = await _firebaseService.UploadFilesAsync(FirebaseConstants.BucketPublic, FirebaseConstants.ImagesKitsFolder + $"/{DTO.Id}", null);
                if (!fileServiceResponse.Succeeded) return StatusCode(fileServiceResponse.StatusCode, new { status = fileServiceResponse.Status, details = fileServiceResponse.Details });
            }
            #endregion
            serviceResponse = await _kitService.UpdateAsync(DTO);
            if (!serviceResponse.Succeeded)
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, detail = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }
        [HttpDelete]
        [Route("{id:int}")]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> RemoveByIdAsync(int id)
        {
            var serviceResponse = await _kitService.RemoveAsync(id);
            if (!serviceResponse.Succeeded)
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }

        [HttpPut]
        [Route("Restore/{id:int}")]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> RestoreByIdAsync(int id)
        {
            var serviceResponse = await _kitService.RestoreByIdAsync(id);
            if (!serviceResponse.Succeeded)
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });

            return Ok(new { status = serviceResponse.Status, detail = serviceResponse.Details });
        }
        #endregion
    }
}
