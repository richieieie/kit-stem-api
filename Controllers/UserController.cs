using System.Net;
using System.Security.Claims;
using kit_stem_api.Data;
using kit_stem_api.Models.Domain;
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
        private readonly KitStemDbContext _dbContext;
        public UserController(IUserService userService, KitStemDbContext dbContext)
        {
            _userService = userService;
            _dbContext = dbContext;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO requestBody)
        {
            var serviceResponse = await _userService.RegisterAsync(requestBody);
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
                return BadRequest(new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}