using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using system_backend.Const;
using system_backend.Data;
using system_backend.Models.Dtos;
using system_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace system_backend.Controllers.Agents
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        protected ApiRespose _response;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;


        public ExpensesController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new();
        }
        [HttpGet("GetExpenses")]
        [Authorize(Roles = Roles.User_Role + "," + Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetExpenses(string id,DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var expenses = new List<ExpensesPayments>() { };
                if (startDate == null)
                {
                    expenses = await _db.Expenses.Where(i => i.AgentId == id).ToListAsync();

                }
                else
                {

                    expenses = await _db.Expenses.Where(i => i.AgentId == id && i.Date >= startDate && i.Date < endDate).ToListAsync();
                }
                _response.Result = _mapper.Map<List<ExpenseDTO>>(expenses);
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
        [HttpGet("GetExpense")]
        [Authorize(Roles = Roles.User_Role)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetExpense(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var expense = await _db.Expenses.FindAsync(id);
                if (expense == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<ExpenseDTO>(expense);
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
        [HttpPost("CreateExpense")]
        [Authorize(Roles = Roles.User_Role)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> CreateExpense([FromBody] ExpenseDTO expenseModel)
        {
            try
            {

                if (expenseModel == null)
                {
                    return BadRequest();
                }

                var expense = _mapper.Map<ExpensesPayments>(expenseModel);
                await _db.Expenses.AddAsync(expense);
                await _db.SaveChangesAsync();
                _response.Result = expenseModel;
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
        [HttpPut("UpdateExpense")]
        [Authorize(Roles = Roles.User_Role)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> UpdateExpense(int id, [FromBody] ExpenseDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                var expense = _mapper.Map<ExpensesPayments>(updateDTO);

                _db.Expenses.Update(expense);
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
        [HttpDelete("DeleteExpense")]
        [Authorize(Roles = Roles.User_Role + "," + Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiRespose>> DeleteExpense(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var expense = await _db.Expenses.FindAsync(id);
                if (expense == null)
                {
                    return NotFound();
                }
                _db.Expenses.Remove(expense);
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
