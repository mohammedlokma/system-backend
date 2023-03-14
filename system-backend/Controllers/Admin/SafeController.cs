using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using system_backend.Const;
using system_backend.Data;
using system_backend.Models;
using system_backend.Models.Dtos;

namespace system_backend.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin_Role)]
    public class SafeController : ControllerBase
    {
        protected ApiRespose _response;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public SafeController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new();
        }
        [HttpGet("GetSafe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetSafe()
        {
            try
            {

                var total = await _db.Safe.FirstOrDefaultAsync();
                var totalIncome = await _db.SafeInputs.SumAsync(i => i.Price);
                var totalOutcome = await _db.SafeOutputs.SumAsync(i => i.Price);
                var obj = new SafeDto()
                {
                    Total = total.Total,
                    TotalIncome = totalIncome,
                    TotalOutcome = totalOutcome
                };
                _response.Result = obj;
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
        [HttpGet("GetSafeInputs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetSafeInputs(DateTime startDate,DateTime endDate)
        {
            try
            {

                var inputs = await _db.SafeInputs.Where(i => i.Date >= startDate && i.Date < endDate).ToListAsync();
                _response.Result = inputs;
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
        [HttpGet("GetSafeOutputs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetSafeOutputs(DateTime startDate, DateTime endDate)
        {
            try
            {

                var outputs = await _db.SafeOutputs.Where(i => i.Date >= startDate && i.Date < endDate).ToListAsync();
                _response.Result = outputs;
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
        [HttpPost("AddSafeInput")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> AddSafeInput([FromBody] SafeInputs safeModel)
        {
            try
            {

                if (safeModel == null)
                {
                    return BadRequest();
                }
                var transaction = _db.Database.BeginTransaction();
                try
                {
                    await _db.SafeInputs.AddAsync(safeModel);
                    await UpdateSafe(safeModel.Price,true);
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
        [HttpPost("AddSafeOutput")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> AddSafeOutput([FromBody] SafeOutputs safeModel)
        {
            try
            {

                if (safeModel == null)
                {
                    return BadRequest();
                }

                var transaction = _db.Database.BeginTransaction();
                try
                {
                    await _db.SafeOutputs.AddAsync(safeModel);
                    await UpdateSafe(safeModel.Price,false);
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
        [HttpPost("UpdateSafe")]
        public async Task<ActionResult<ApiRespose>> UpdateSafe(double value,bool input)
        {
            try
            {
                if (value == 0)
                {
                    return BadRequest();
                }
                var total = await _db.Safe.AsNoTracking().FirstOrDefaultAsync();
                Safe newValue;
                if (input)
                {
                     newValue = new Safe() { Id = total.Id, Total = total.Total + value };
                }
                else
                {
                     newValue = new Safe() { Id = total.Id, Total = total.Total - value };

                }
                _db.Safe.Attach(newValue).Property(x => x.Total).IsModified = true;
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
