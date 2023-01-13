using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using system_backend.Const;
using system_backend.Models;
using system_backend.Models.Dtos;
using system_backend.Repository.Interfaces;

namespace system_backend.Controllers.Agents
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentController : ControllerBase
    {
        protected ApiRespose _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public AgentController(IUnitOfWork unit, IMapper mapper)
        {
            _unitOfWork = unit;
            _mapper = mapper;
            _response = new();
        }
        [HttpGet("GetAgents")]
        [Authorize(Roles = Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        [Authorize(Roles = Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiRespose>> GetAgent(string id)
        {
            try
            {
                if (id is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var agent = await _unitOfWork.Agents.GetAsync(u => u.Id == id, includeProperties: "Expenses,Coupons");
                if (agent == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<AgentModel>(agent);
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
        [Authorize(Roles = Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> CreateAgent([FromBody] RegisterAgentModel agentModel)
        {
            try
            {

                if (agentModel == null)
                {
                    return BadRequest(agentModel);
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[HttpDelete("DeleteEmployee")]
        //public async Task<ActionResult<ApiRespose>> DeleteEmployee(int id)
        //{
        //    try
        //    {
        //        if (id == 0)
        //        {
        //            return BadRequest();
        //        }
        //        var employee = await _unitOfWork.Employees.FindAsync(u => u.Id == id);
        //        if (employee == null)
        //        {
        //            return NotFound();
        //        }
        //        await _unitOfWork.Employees.RemoveAsync(employee);
        //        await _unitOfWork.SaveAsync();
        //        _response.StatusCode = HttpStatusCode.NoContent;
        //        _response.IsSuccess = true;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages
        //             = new List<string>() { ex.ToString() };
        //    }
        //    return _response;
        //}

    }
}
