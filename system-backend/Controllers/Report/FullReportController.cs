using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Dynamic;
using System.Net;
using system_backend.Const;
using system_backend.Data;
using system_backend.Models;
using system_backend.Models.Dtos;
using system_backend.Repository.Interfaces;
using system_backend.Services;

using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace system_backend.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class FullReportController : ControllerBase
    {
        protected ApiRespose _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        public FullReportController( IUnitOfWork unitOfWork, IMapper mapper, ApplicationDbContext db,IConfiguration configuration)
        {
            _response = new();
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _db = db;
            _configuration = configuration;
        }
        [HttpGet("GetReportItems")]
        [Authorize(Roles = Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetReportItems()
        {
            try
            {

                var items = await _db.ReportItems.ToListAsync();
                _response.Result = _mapper.Map<List<ReportItemsDTO>>(items);
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
        [HttpPost("CreateReportItem")]
        [Authorize(Roles = Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> CreateReportItem([FromBody] ReportItemsDTO itemModel)
        {
            try
            {

                if (itemModel == null)
                {
                    return BadRequest();
                }
                var transaction = _db.Database.BeginTransaction();
                try
                {
                    var item = _mapper.Map<ReportItems>(itemModel);
                    await _db.ReportItems.AddAsync(item);
                    var connectionString = _configuration.GetConnectionString("DefaultConnection");
                    string queryString =
                        "ALTER TABLE FullReport ADD " + item.Name +
                        " varchar(255);";
                    using (SqlConnection connection = new SqlConnection(
                               connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(queryString, connection))
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }
   
                    }
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {

                    transaction.Rollback();
                }

                
                _response.Result = itemModel;
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
        [HttpDelete("DeleteReportItem")]
        [Authorize(Roles = Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiRespose>> DeleteReportItem(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var item = await _db.ReportItems.FindAsync(id);
                if (item == null)
                {
                    return NotFound();
                }
                var transaction = _db.Database.BeginTransaction();
                try
                {
                    _db.ReportItems.Remove(item);

                    var connectionString = _configuration.GetConnectionString("DefaultConnection");
                    string queryString =
                        "ALTER TABLE FullReport DROP COLUMN " + item.Name + ";";
                    using (SqlConnection connection = new SqlConnection(
                               connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(queryString, connection))
                        {
                            connection.Open();
                            cmd.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                    await _db.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception)
                {

                    transaction.Rollback();
                }
               
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
        [HttpPost("AddReportItemToCompany")]
        [Authorize(Roles = Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> AddReportItemToCompany([FromBody] CompanyReportItemsDTO itemModel)
        {
            try
            {

                if (itemModel == null)
                {
                    return BadRequest();
                }
                foreach (var item in itemModel.CompanyReportItems) {
                    var newItem = new CompanyReportItems()
                    {
                        CompanyId = itemModel.CompanyId,
                        ReportItemId = item.ReportItemId
                    };
                    _db.CompanyReportItems.Add(newItem);
                }
               
                await _db.SaveChangesAsync();
                _response.Result = itemModel;
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
        [HttpGet("GetCompanyReportItems")]
        [Authorize(Roles = Roles.Admin_Role + ","+ Roles.Company_Role)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> GetCompanyReportItems(string id)
        {
            try
            {

                var items = await (from a in _db.CompanyReportItems
                                   join s in _db.ReportItems on a.ReportItemId equals s.Id
                                   where a.CompanyId == id
                                   select new ReportItemsDTO() 
                                   {
                                       ArabicName=s.ArabicName,
                                       Name=s.Name,
                                       Type = s.Type
                                       
                                   } 
                                   ).ToListAsync();
                _response.Result = _mapper.Map<List<ReportItemsDTO>>(items);
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
        [HttpPost("AddRow")]
        [Authorize(Roles = Roles.Admin_Role)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiRespose>> AddRow([FromBody] Dictionary<string,string> jsonObject )
        {
            try
            {

                if (jsonObject == null)
                {
                    return BadRequest();
                }

                //var s = System.Text.Json.JsonSerializer.Serialize(jsonObject.ToString());
                //var d = jsonObject.row;
                //string c = JsonConvert.DeserializeObject(jsonObject,typeof(ExpandoObject));
                //string jsonResponse = jsonObject.ToString().Trim().TrimStart('{').TrimEnd('}');
                //var data2 = JsonConvert.DeserializeObject<string>(d.ToString());    
                //var data = JsonConvert.DeserializeObject<dynamic>(s);
                // var data2 = JsonConvert.DeserializeObject<dynamic>(data.ToString());

                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                var query = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME ='FullReport'";

                SqlConnection connection = new SqlConnection(connectionString);

                SqlCommand cmd = new SqlCommand(query, connection);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                connection.Open();
                da.Fill(dt);
                var columns = dt.AsEnumerable().Select(i => i.ItemArray).ToList();

              
                var keys = "";
                var vals = "";
                for (int i = 0; i < columns.Count; ++i)
                {
                    if(jsonObject.ContainsKey(columns[i][0].ToString()))
                    {
                        keys += columns[i][0].ToString();
                        if (i < columns.Count - 1) keys += ",";
                        vals += "'";
                        vals += jsonObject[columns[i][0].ToString()];
                        vals += "'";
                        if (i < columns.Count - 1) vals += ",";
                    }
                }
                

                string queryString =
                   "INSERT INTO FullReport (" + keys + ") VALUES(" + vals + ");";

                SqlCommand cmd2 = new SqlCommand(queryString, connection);
                cmd2.ExecuteNonQuery();
                connection.Close();
                // await _db.SaveChangesAsync();
                _response.Result = jsonObject;
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
