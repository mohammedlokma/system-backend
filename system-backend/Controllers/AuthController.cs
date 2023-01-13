using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using system_backend.Const;
using system_backend.Data;
using system_backend.Models.Dtos;
using system_backend.Repository.Interfaces;
using system_backend.Services;

namespace system_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthServices _authServices;
        private readonly ApplicationDbContext _db;
        public AuthController(IAuthServices authServices,ApplicationDbContext db)
        {
            _authServices = authServices;
            _db = db;
        }

        [Authorize(Roles = Roles.Admin_Role)]
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
            _db.SaveChangesAsync();

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
