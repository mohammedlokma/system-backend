using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Build.Framework;
using System.Net;
using system_backend.Const;
using system_backend.Data;
using system_backend.Models;
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
        protected ApiRespose _response;

        public AuthController(IAuthServices authServices, ApplicationDbContext db )
        {
            _authServices = authServices;
            _db = db;
            _response = new();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                username = result.Username,
                userDisplayName = result.UserDisplayName
            });
        }
        [Authorize(Roles = Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("ResetPassword")]
        public async Task<ActionResult<ApiRespose>> ResetPassword(PasswordResetDto passwordResetDto)
        {
            try
            {
                if (passwordResetDto.userId is null || passwordResetDto.NewPassword is null)
                {
                    return BadRequest();
                }
                  
                var result = await _authServices.ResetPasswordAsync(passwordResetDto);
                   if (result is null)
                    {
                            _response.ErrorMessages
                                     = new List<string>() { "حدث خطأ بالرجاء التأكد من البيانات أو إعادة المحاولة" };
                    return _response;
                }
                    await _db.SaveChangesAsync();
                
                
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
