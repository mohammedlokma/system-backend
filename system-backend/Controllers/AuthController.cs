using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using system_backend.Models;
using system_backend.Services;

namespace system_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthServices _authServices;
        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authServices.RegisterAsync(model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);

            }
            
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var result = await _authServices.LoginAsync(model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);

            }
            return Ok(new
            {
                token = result.Token,
                exp = result.ExpiresOn,
                role = result.Role,
                username = result.Username
            });
        }
    }
}
