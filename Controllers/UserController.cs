using System.Net;
using kit_stem_api.Models.DTO;
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
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO requestBody)
        {
            var (succeed, message) = await _userService.RegisterAsync(requestBody);
            if (!succeed)
            {
                return BadRequest(new { status = "fail", message });
            }

            return Ok(new { status = "success", message });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO requestBody)
        {
            var (succeed, message) = await _userService.LoginAsync(requestBody);
            if (!succeed)
            {
                return BadRequest(new { status = "fail", message });
            }

            var cookieOptions = new CookieOptions()
            {
                Expires = DateTime.Now.AddMinutes(30),
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true
            };
            Response.Cookies.Append("accessToken", message, cookieOptions);

            return Ok(new { status = "success" });
        }
    }
}