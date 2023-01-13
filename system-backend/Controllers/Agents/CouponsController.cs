using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using system_backend.Const;
using system_backend.Models.Dtos;
using system_backend.Models;
using AutoMapper;
using system_backend.Repository.Interfaces;
using system_backend.Repository;
using system_backend.Data;

namespace system_backend.Controllers.Agents
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.User_Role)]

    public class CouponsController : ControllerBase { 
            protected ApiRespose _response;
            private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unit;
            private readonly IMapper _mapper;


        public CouponsController(ApplicationDbContext db,IUnitOfWork unit, IMapper mapper)
        {
            _db = db;
            _unit = unit;
            _mapper = mapper;
            _response = new();
        }
        [HttpPost("CreateCoupon")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> CreateCoupon([FromBody] CouponDTO couponModel)
        {
            try
            {

                if (couponModel == null)
                {
                    return BadRequest(couponModel);
                }

                var coupon = _mapper.Map<CouponsPayments>(couponModel);
                await _db.Coupons.AddAsync(coupon);
                await _db.SaveChangesAsync();
                _response.Result = couponModel;
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
        [HttpPut("UpdateCoupon")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> UpdateCoupon(int id, [FromBody] CouponDTO updateDTO)
        {
            try
            {
                if (updateDTO == null || id != updateDTO.Id)
                {
                    return BadRequest();
                }

                var coupon = _mapper.Map<CouponsPayments>(updateDTO);

                _db.Coupons.Update(coupon);
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
        [HttpDelete("DeleteCoupon")]
        public async Task<ActionResult<ApiRespose>> DeleteCoupon(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var coupon = await _db.Coupons.FindAsync(id);
                if (coupon == null)
                {
                    return NotFound();
                }
                _db.Coupons.Remove(coupon);
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
