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

namespace system_backend.Controllers.Company
{
    [Route("api/[controller]")]

    [ApiController]
    public class BillsController : ControllerBase
    {
        protected ApiRespose _response;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
            
        public BillsController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new();
        }
      
        [HttpPost("CreateBill")]
        [Authorize(Roles = Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> CreateBill([FromBody] BillsDTO billModel)
        {
            try
            {

                if (billModel == null)
                {
                    return BadRequest();
                }

                var bill = _mapper.Map<Bills>(billModel);
                await _db.Bills.AddAsync(bill);
                await _db.SaveChangesAsync();
                _response.Result = billModel;
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

        [HttpGet("GetBills")]
        [Authorize(Roles = Roles.User_Role + "," + Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetBills(string id, DateTime startDate, DateTime endDate)
        {
            try
            {

                var bills = await _db.Bills.Where(i => i.CompanyId == id && i.Date >= startDate && i.Date < endDate).ToListAsync();
                _response.Result = _mapper.Map<List<BillsDTO>>(bills);
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
        [HttpPut("UpdateBill")]
        [Authorize(Roles = Roles.User_Role)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> UpdateBill(int id, [FromBody] BillsDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                var bill = _mapper.Map<Bills>(updateDTO);

                _db.Bills.Update(bill);
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
        [HttpDelete("DeleteBill")]
        [Authorize(Roles = Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiRespose>> DeleteBill(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var bill = await _db.Bills.FindAsync(id);
                if (bill == null)
                {
                    return NotFound();
                }
                _db.Bills.Remove(bill);
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
