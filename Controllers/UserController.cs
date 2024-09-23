using System.Net;
using System.Security.Claims;
using Google.Apis.Auth;
using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
using kit_stem_api.Models.DTO;
using kit_stem_api.Services;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGoogleService _googleService;
        private readonly KitStemDbContext _dbContext;
        public UserController(IUserService userService, IGoogleService googleService, KitStemDbContext dbContext)
        {
            _userService = userService;
            _googleService = googleService;
            _dbContext = dbContext;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO requestBody)
        {
            var serviceResponse = await _userService.RegisterAsync(requestBody, "customer");
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO requestBody)
        {
            var serviceResponse = await _userService.LoginAsync(requestBody);
            if (!serviceResponse.Succeeded)
            {
                return Unauthorized(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost("LoginWithGoogle")]
        public async Task<IActionResult> LoginWithGoogle(GoogleCredentialsDTO googleCredentialsDTO)
        {
            var serviceResponse = await _googleService.VerifyGoogleTokenAsync(googleCredentialsDTO);
            if (!serviceResponse.Succeeded)
            {
                return Unauthorized(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            serviceResponse = await _userService.LoginWithGoogleAsync((GoogleJsonWebSignature.Payload)serviceResponse.Details!["payload"]);
            if (!serviceResponse.Succeeded)
            {
                return Unauthorized(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet("UserProfile")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> UserProfile()
        {
            var userName = User.FindFirst(ClaimTypes.Email)?.Value;
            var serviceResponse = await _userService.GetProfileAsync(userName);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut("UpdatevProfile")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> UpdateUserProfile(UserUpdateDTO userUpdateDTO)
        {
            var userName = User.FindFirst(ClaimTypes.Email)?.Value;

            var serviceResponse = await _userService.UpdateProfileAsync(userName, userUpdateDTO);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Route("/RefreshToken/{refreshToken:guid}")]
        public async Task<IActionResult> RefreshToken(Guid refreshToken)
        {
            var serviceResponse = await _userService.RefreshTokenAsync(refreshToken);
            if (!serviceResponse.Succeeded)
            {
                return Unauthorized(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}