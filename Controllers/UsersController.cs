using System.Security.Claims;
using Google.Apis.Auth;
using kit_stem_api.Constants;
using kit_stem_api.Models.DTO;
using kit_stem_api.Models.DTO.Request;
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
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterDTO requestBody)
        {
            var serviceResponse = await _userService.RegisterAsync(requestBody, UserConstants.CustomerRole);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var subject = "Chào mừng bạn đến với shop!";
            var body = $"Xin chào, Cảm ơn bạn đã đăng ký tài khoản tại KitStemHub! Chúng tôi rất vui khi có bạn là một phần của cộng đồng mua sắm của chúng tôi. Hãy khám phá và tận hưởng những ưu đãi đặc biệt dành riêng cho thành viên mới. Chúc bạn có trải nghiệm mua sắm tuyệt vời!";
            await _emailService.SendEmail(requestBody.Email!, subject, body);

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }


        [HttpPost]
        [Route("Register/Staff")]
        public async Task<IActionResult> RegisterStaffAsync([FromBody] UserRegisterDTO requestBody)
        {
            var serviceResponse = await _userService.RegisterAsync(requestBody, UserConstants.StaffRole);
            if (!serviceResponse.Succeeded)
            {
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var subject = "Chào mừng bạn đến với shop!";
            var body = $"Chào mừng bạn đến với KitStemHub! Chúc mừng bạn đã chính thức trở thành một thành viên trong đội ngũ của chúng tôi. Hy vọng chúng ta sẽ hợp tác hiệu quả và gặt hái nhiều thành công cùng nhau!";
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
        [Route("Customers")]
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