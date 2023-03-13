using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using system_backend.Data;
using system_backend.Models.Dtos;
using system_backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using system_backend.Const;
using Newtonsoft.Json.Linq;
using system_backend.Repository.Interfaces;

namespace system_backend.Controllers.Company
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin_Role)]

    public class PaymentsController : ControllerBase
    {
        protected ApiRespose _response;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentsController(ApplicationDbContext db, IMapper mapper,IUnitOfWork unitOfWork)
        {
            _db = db;
            _mapper = mapper;
            _unitOfWork = unitOfWork;  
            _response = new();
        }

        [HttpPost("CreatePayment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> CreatePayment([FromBody] PaymentsDTO paymentModel)
        {
            try
            {
                var transaction = _db.Database.BeginTransaction();
                try
                {
                    if (paymentModel == null)
                    {
                        return BadRequest();
                    }
                    var total = await _db.Safe.AsNoTracking().FirstOrDefaultAsync();
                    var newValue = new Safe() { Id = total.Id, Total = total.Total + paymentModel.Price };
                    _db.Safe.Attach(newValue).Property(x => x.Total).IsModified = true;
                    var company = await _unitOfWork.Companies.GetAsync(i => i.Id == paymentModel.CompanyId);
                    if (company is null)
                    {
                        return BadRequest();
                    }
                    company.Account += paymentModel.Price;
                    var payment = _mapper.Map<CompanyPayments>(paymentModel);
                    await _db.CompanyPayments.AddAsync(payment);
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _response.ErrorMessages
                    = new List<string>() { ex.ToString() };
                    return _response;

                }
                _response.Result = paymentModel;
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
        [HttpGet("GetPayments")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetPayments(string id, DateTime startDate, DateTime endDate)
        {
            try
            {

                var bills = await _db.CompanyPayments.Where(i => i.CompanyId == id && i.Date >= startDate && i.Date < endDate).ToListAsync();
                _response.Result = _mapper.Map<List<PaymentsDTO>>(bills);
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
        [HttpPut("UpdatePayment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> UpdatePayment(int id, [FromBody] PaymentsDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                var payment = _mapper.Map<CompanyPayments>(updateDTO);

                _db.CompanyPayments.Update(payment);
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
        [HttpDelete("DeletePayment")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiRespose>> DeletePayment(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var payment = await _db.CompanyPayments.FindAsync(id);
                if (payment == null)
                {
                    return NotFound();
                }
                _db.CompanyPayments.Remove(payment);
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
