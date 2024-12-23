using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Google.Apis.Auth;
using KSH.Api.Constants;
using KSH.Api.Models.DTO;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Services;
using KSH.Api.Services.IServices;
using KSH.Api.Utils.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KSH.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGoogleService _googleService;
        private readonly IEmailService _emailService;
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        public UsersController(IUserService userService, IGoogleService googleService, IEmailService emailService, IEmailTemplateProvider emailTemplateProvider)
        {
            _userService = userService;
            _googleService = googleService;
            _emailService = emailService;
            _emailTemplateProvider = emailTemplateProvider;
        }

        [HttpGet]
        [Route("Genders")]
        public IActionResult GetGenders()
        {
            var serviceResponse = _userService.GetGenderOptions();
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] CustomerRegisterDTO requestBody)
        {
            var (serviceResponse, token) = await _userService.RegisterAsync(requestBody, UserConstants.CustomerRole);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var subject = "Chào mừng bạn đến với shop!";
            var clientBaseUrl = Request.Scheme == "https" ? "https://kit-stem-hub-fe-customer.vercel.app" : "http://localhost:5173";
            var verifyUrl = $"{clientBaseUrl}/verify?email={Uri.EscapeDataString(requestBody.Email!)}&token={Uri.EscapeDataString(token!)}";
            var body = _emailTemplateProvider.GetRegisterTemplate(requestBody.Email!, "KitStemHub", verifyUrl!);
            await _emailService.SendEmail(requestBody.Email!, subject, body);

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("Email/Verify")]
        [ActionName(nameof(VerifyEmail))]
        public async Task<IActionResult> VerifyEmail([FromQuery] string email, [FromQuery] string token)
        {
            var serviceResponse = await _userService.VerifyEmail(email, token);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut]
        [Route("Profile/Staff/{staffUserName}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> UpdateStaffAsync(string staffUserName, UserUpdateDTO userUpdateDTO)
        {
            var serviceResponse = await _userService.UpdateAsync(staffUserName, userUpdateDTO);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("Profile/Staff/{staffUserName}")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetStaffAsync(string staffUserName)
        {
            var serviceResponse = await _userService.GetAsync(staffUserName!);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Route("Register/Staff")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> RegisterStaffAsync([FromBody] StaffRegisterDTO requestBody)
        {
            var (serviceResponse, token) = await _userService.RegisterAsync(requestBody, UserConstants.StaffRole);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

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

            serviceResponse = await _userService.LoginWithGoogleAsync((GoogleJsonWebSignature.Payload)serviceResponse.Details![ServiceResponse.ToKebabCase("payload")]);
            if (!serviceResponse.Succeeded)
            {
                return Unauthorized(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Route("SendPasswordResetToken")]
        public async Task<IActionResult> SendPasswordResetTokenAsync([FromBody] SendPasswordResetTokenDTO sendPasswordResetTokenDTO)
        {
            var (serviceResponse, token) = await _userService.GeneratePasswordResetTokenAsync(sendPasswordResetTokenDTO.Email!);
            if (!serviceResponse.Succeeded)
            {
                return Unauthorized(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var subject = "Cài đặt lại mật khẩu!";
            var clientBaseUrl = Request.Scheme == "https" ? "https://kit-stem-hub-fe-customer.vercel.app" : "http://localhost:5173";
            var passwordResetUrl = $"{clientBaseUrl}/password/reset?email={Uri.EscapeDataString(sendPasswordResetTokenDTO.Email!)}&token={Uri.EscapeDataString(token!)}";
            var body = _emailTemplateProvider.GetPasswordResetTemplate(sendPasswordResetTokenDTO.Email!, "KitStemHub", passwordResetUrl!);
            await _emailService.SendEmail(sendPasswordResetTokenDTO.Email!, subject, body);

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] PasswordResetDTO passwordResetDTO)
        {
            passwordResetDTO.Email = Uri.UnescapeDataString(passwordResetDTO.Email!);
            passwordResetDTO.Token = Uri.UnescapeDataString(passwordResetDTO.Token!);

            var serviceResponse = await _userService.ResetPasswordAsync(passwordResetDTO);
            if (!serviceResponse.Succeeded)
            {
                return Unauthorized(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("Profile")]
        [Authorize(Roles = "customer,staff")]
        public async Task<IActionResult> GetAsync()
        {
            var userName = User.FindFirst(ClaimTypes.Email)?.Value;
            var serviceResponse = await _userService.GetAsync(userName!);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetAsync([FromQuery] UserManagerGetDTO userManagerGetDTO)
        {
            var serviceResponse = await _userService.GetAllAsync(userManagerGetDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPut]
        [Route("Profile")]
        [Authorize(Roles = "customer")]
        public async Task<IActionResult> UpdateAsync(UserUpdateDTO userUpdateDTO)
        {
            var userName = User.FindFirst(ClaimTypes.Email)?.Value;

            var serviceResponse = await _userService.UpdateAsync(userName!, userUpdateDTO);
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

        [HttpDelete]
        [Route("{userName}")]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> RemoveByEmailAsync(string userName)
        {
            var serviceResponse = await _userService.RemoveByEmailAsync(userName);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return NoContent();
        }

        [HttpPut]
        [Route("Restore/{userName}")]
        // [Authorize(Roles = "manager")]
        public async Task<IActionResult> RestoreByEmailAsync(string userName)
        {
            var serviceResponse = await _userService.RestoreByEmailAsync(userName);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Route("RegisterWithRole/Only-For-Testing/{role}")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO requestBody, string role)
        {
            var (serviceResponse, token) = await _userService.RegisterAsync(requestBody, role);
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