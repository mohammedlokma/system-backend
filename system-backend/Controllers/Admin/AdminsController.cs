using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Text;
using System.Net;
using system_backend.Const;
using system_backend.Data;
using system_backend.Models;
using system_backend.Models.Dtos;
using system_backend.Repository.Interfaces;

namespace system_backend.Controllers.Admin
{
    [Authorize(Roles = Roles.Admin_Role)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        protected ApiRespose _response;
        private readonly IUnitOfWork _unitOfWork;
        
      

        public AdminsController(IUnitOfWork unit)
        {
            _unitOfWork = unit;
           
            _response = new();
        }

        [HttpGet("GetAdmins")]
        //[ResponseCache(CacheProfileName = "Default30")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiRespose>> GetAdmins()
        {
            try
            {

                var adminsDto = await _unitOfWork.Admins.GetAdmins();
                _response.Result = adminsDto;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response.Result);
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
