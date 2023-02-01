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
using system_backend.Services;

namespace system_backend.Controllers.Admin
{
   [Authorize(Roles = Roles.Admin_Role)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        protected ApiRespose _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthServices _authServices;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        public AdminsController(IUnitOfWork unit,IAuthServices authServices,
            UserManager<ApplicationUser> userManager, IMapper mapper,ApplicationDbContext db)
        {
            _unitOfWork = unit;
            _authServices = authServices;
            _userManager = userManager;
            _mapper = mapper;
            _response = new();
            _db = db;
        }
        [HttpPost("CreateAdmin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAdmin([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _authServices.RegisterAsync(model);
            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);

            }
            await _unitOfWork.SaveAsync();

            return Ok(result);
        }
        [HttpPut("UpdateAdmin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> UpdateAdmin(string id, [FromBody] AdminUpdateDTO updateDTO)
        {
            try
            {

                var admin = await _userManager.FindByIdAsync(id);
                if (updateDTO == null || admin == null)
                {
                    return BadRequest();
                }
                admin.UserName = updateDTO.UserName;
                admin.UserDisplayName = updateDTO.UserDisplayName;
                await _userManager.UpdateAsync(admin);
                await _unitOfWork.SaveAsync();
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpDelete("DeleteAdmin")]
        public async Task<ActionResult<ApiRespose>> DeleteAdmin(string id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }
                var admin = await _userManager.FindByIdAsync(id);
                if (admin == null)
                {
                    return NotFound();
                }
                await _userManager.DeleteAsync(admin);
                await _unitOfWork.SaveAsync();
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

        [HttpGet("GetServicePlaces")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetServicePlaces()
        {
            try
            {
               
                var places = await (from place in _db.ServicePlaces
                                    select new GetServicePlacesDTO
                                    {
                                        Id = place.Id,
                                        Name = place.Name,
                                        NumOfAgents = _db.AgentServicePlaces.Where(i=>i.ServicePlacesId == place.Id).Count()

                                    }
                             ).ToListAsync();
                _response.Result = places;
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
        [HttpGet("GetAgentServicePlaces")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetAgentServicePlaces(int id)
        {
            try
            {
                IEnumerable<AgentDTO> agents = await _unitOfWork.Agents.GetAgentPlacesAsync(id);
                _response.Result = agents;
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
        [HttpPost("CreateServicePlace")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> CreateServicePlace([FromBody] ServicePlacesCreateDTO createModel)
        {
            try
            {

                if (createModel == null)
                {
                    return BadRequest(createModel);
                }

                var place = _mapper.Map<ServicePlaces>(createModel);
                await _db.ServicePlaces.AddAsync(place);
                await _db.SaveChangesAsync();
                _response.Result = createModel;
                _response.StatusCode = HttpStatusCode.Created;
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
        [HttpPut("UpdateServicePlace")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> UpdateServicePlace(int id, [FromBody] ServicePlacesUpdateDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                var place = _mapper.Map<ServicePlaces>(updateDTO);

                _db.ServicePlaces.Update(place);
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

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("DeleteServicePlace")]
        public async Task<ActionResult<ApiRespose>> DeleteServicePlace(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var place = await _db.ServicePlaces.FindAsync(id);
                if (place == null)
                {
                    return NotFound();
                }
                _db.ServicePlaces.Remove(place);
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
