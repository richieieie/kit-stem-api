using System.Security.Claims;
using Google.Apis.Auth;
using kit_stem_api.Constants;
using kit_stem_api.Models.DTO;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGoogleService _googleService;
        private readonly IEmailService _emailService;
        public UsersController(IUserService userService, IGoogleService googleService, IEmailService emailService)
        {
            _userService = userService;
            _googleService = googleService;
            _emailService = emailService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO requestBody)
        {
            var serviceResponse = await _userService.RegisterAsync(requestBody, UserConstants.CustomerRole);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var subject = "Welcome to our shop!";
            var body = "Thank you for registering, We're excited to have you visit our shop. Explore our latest products and enjoy exclusive offers just for you!";
            await _emailService.SendEmail(requestBody.Email!, subject, body);

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Route("Login")]
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

        [HttpGet]
        [Route("Profile")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> UserProfile()
        {
            var userName = User.FindFirst(ClaimTypes.Email)?.Value;
            var serviceResponse = await _userService.GetProfileAsync(userName!);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut]
        [Route("Profile")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> UpdateUserProfile(UserUpdateDTO userUpdateDTO)
        {
            var userName = User.FindFirst(ClaimTypes.Email)?.Value;

            var serviceResponse = await _userService.UpdateProfileAsync(userName!, userUpdateDTO);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }
            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Route("RefreshToken/{refreshToken:guid}")]
        public async Task<IActionResult> RefreshToken(Guid refreshToken)
        {
            var serviceResponse = await _userService.RefreshTokenAsync(refreshToken);
            if (!serviceResponse.Succeeded)
            {
                return Unauthorized(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Route("RegisterWithRole/Only-For-Testing/{role}")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO requestBody, string role)
        {
            var serviceResponse = await _userService.RegisterAsync(requestBody, role);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var subject = "Welcome to our shop!";
            var body = "Thank you for registering, We're excited to have you visit our shop. Explore our latest products and enjoy exclusive offers just for you!";
            await _emailService.SendEmail(requestBody.Email!, subject, body);

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}