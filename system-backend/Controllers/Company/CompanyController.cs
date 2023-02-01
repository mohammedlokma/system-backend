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
using system_backend.Repository.Interfaces;

namespace system_backend.Controllers.Company
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.Admin_Role)]

    public class CompanyController : ControllerBase
    {
        protected ApiRespose _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private ApplicationDbContext _db;
        public CompanyController(IUnitOfWork unit, IMapper mapper, ApplicationDbContext db)
        {
            _unitOfWork = unit;
            _mapper = mapper;
            _db = db;
            _response = new();
        }
        [HttpPost("CreateCompany")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> CreateCompany([FromBody] RegisterCompanyModel companyModel)
        {
            try
            {

                if (companyModel == null)
                {
                    return BadRequest();
                }


                var result = await _unitOfWork.Companies.RegisterUserAsync(companyModel);
                if (!result.IsAuthenticated)
                {
                    return BadRequest(result.Message);
                }
                _response.Result = companyModel;
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

        [HttpGet("GetCompany")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetCompany(string id)
        {
            try
            {
                if (id is null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                //var company = await _unitOfWork.Companies.GetAsync(u => u.Id == id, includeProperties: "Payments,Bills");
                var company = await _unitOfWork.Companies.GetCompanyAsync(id);
                if (company == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = company;
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
        [HttpGet("GetCompanies")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetCompanies()
        {
            try
            {

                IEnumerable<CompanyDTO> companies = await _unitOfWork.Companies.GetCompaniesAsync();
                _response.Result = companies;
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
        [HttpPut("UpdateCompany")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> UpdateCompany(string id, [FromBody] CompanyUpdateModel updateDTO)
        {
            try
            {

                var company = await _unitOfWork.Companies.GetAsync(u => u.Id == id);
                if (updateDTO == null || company == null)
                {
                    return BadRequest();
                }


                var result = await _unitOfWork.Companies.UpdateAsync(updateDTO);
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
        [HttpDelete("DeleteCompany")]
        public async Task<ActionResult<ApiRespose>> DeleteCompany(string id)
        {
            try
            {
                if (id is null)
                {
                    return BadRequest();
                }
                var company = await _unitOfWork.Companies.GetAsync(i => i.Id == id);
                if (company == null)
                {
                    return NotFound();
                }
                await _unitOfWork.Companies.DeleteCompanyAsync(id);
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
