using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net;
using system_backend.Const;
using system_backend.Data;
using system_backend.Models;
using system_backend.Models.Dtos;
using system_backend.Repository.Interfaces;

namespace system_backend.Controllers.Agents
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin_Role)]

    public class AgentController : ControllerBase
    {
        protected ApiRespose _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ApplicationDbContext _db;


        public AgentController(IUnitOfWork unit, IMapper mapper,ApplicationDbContext db)
        {
            _unitOfWork = unit;
            _mapper = mapper;
            _db = db;
            _response = new();
        }       
        [HttpGet("GetAgents")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetAgents()
        {
            try
            {

                IEnumerable<AgentDTO> agents = await _unitOfWork.Agents.GetAgentsAsync();
                //IEnumerable<AgentDTO> agentsDto = _mapper.Map<List<AgentDTO>>(agents);
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

        [HttpGet("GetAgent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetAgent(string id)
        {
            try
            {
                if (id is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var agent = await _unitOfWork.Agents.GetAgentAsync(id);
                if (agent == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = agent;
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
        [HttpPost("CreateAgent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> CreateAgent([FromBody] RegisterAgentModel agentModel)
        {
            try
            {

                if (agentModel == null)
                {
                    return BadRequest();
                }


               var result = await _unitOfWork.Agents.RegisterUserAsync(agentModel);
                if (!result.IsAuthenticated)
                {
                    return BadRequest(result.Message);
                }
                _response.Result = agentModel;
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

        [HttpPut("UpdateAgent")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> UpdateAgent(string id, [FromBody] AgentUpdateDTO updateDTO)
        {
            try
            {

                var agent = await _unitOfWork.Agents.GetAsync(u => u.Id == id);
                if (updateDTO == null || agent == null)
                {
                    return BadRequest();
                }

                
                var result = await _unitOfWork.Agents.UpdateAsync(updateDTO);
                _response.Result = result;
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
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
        [HttpDelete("DeleteAgent")]
        public async Task<ActionResult<ApiRespose>> DeleteAgent(string id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }
                var agent = await _unitOfWork.Agents.GetAsync(i => i.Id == id);
                if (agent == null)
                {
                    return NotFound();
                }
                await _unitOfWork.Agents.DeleteAgentAsync(id);
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
        [HttpPost("AddCostToAgent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> AddCostToAgent(string id, float value)
        {
            try
            {

                if (value == 0 || id is null)
                {
                    return BadRequest();
                }

                var transaction = _db.Database.BeginTransaction();
                try
                {
                    var total = await _db.Safe.AsNoTracking().FirstOrDefaultAsync();
                    //if(value > total.Total)
                    //{
                    //    _response.ErrorMessages
                    // = new List<string>() { "الخزنة لا يتوافر بها هذه القيمه" };
                    //    return BadRequest(_response);

                    //}
                    var newValue = new Safe() { Id = total.Id, Total = total.Total - value };
                    _db.Safe.Attach(newValue).Property(x => x.Total).IsModified = true;
                    var agent = await _unitOfWork.Agents.GetAsync(i=>i.Id== id);
                    if (agent is null)
                    {
                        return BadRequest();
                    }
                    agent.custody += value;
                    var safeModel = new SafeOutputs() { 
                    Date= DateTime.Now,
                    details = "تخريج عهده لمندوب ",
                    Price = value,
                    };
                    await _db.SafeOutputs.AddAsync(safeModel);
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    _response.IsSuccess = false;
                    _response.ErrorMessages
                         = new List<string>() { ex.ToString() };
                    return _response;

                }
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
        [HttpPost("WithDrawCostFromAgent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> WithDrawCostFromAgent(string id)
        {
            try
            {

                if ( id is null)
                {
                    return BadRequest();
                }

                var transaction = _db.Database.BeginTransaction();
                try
                {
                    var total = await _db.Safe.AsNoTracking().FirstOrDefaultAsync();
                    var agent = await _unitOfWork.Agents.GetAsync(i => i.Id == id);
                    if (agent is null)
                    {
                        return BadRequest();
                    }
                    var newValue = new Safe() { Id = total.Id, Total = total.Total + agent.custody };
                    _db.Safe.Attach(newValue).Property(x => x.Total).IsModified = true;
                    var safeModel = new SafeInputs()
                    {
                        Date = DateTime.Now,
                        details = "سحب عهده من مندوب ",
                        Price = agent.custody,
                    };
                    await _db.SafeInputs.AddAsync(safeModel);
                    agent.custody =0;
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {

                    transaction.Rollback();
                    _response.IsSuccess = false;
                    _response.ErrorMessages
                         = new List<string>() { ex.ToString() };
                    return _response;
                }
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
    }
}
