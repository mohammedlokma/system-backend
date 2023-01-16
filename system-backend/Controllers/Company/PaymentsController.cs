using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using system_backend.Data;
using system_backend.Models.Dtos;
using system_backend.Models;

namespace system_backend.Controllers.Company
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        protected ApiRespose _response;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public PaymentsController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
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

                if (paymentModel == null)
                {
                    return BadRequest();
                }

                var payment = _mapper.Map<CompanyPayments>(paymentModel);
                await _db.CompanyPayments.AddAsync(payment);
                await _db.SaveChangesAsync();
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
