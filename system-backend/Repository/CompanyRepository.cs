using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using system_backend.Data;
using system_backend.Models;
using system_backend.Models.Dtos;
using system_backend.Repository.Interfaces;
using system_backend.Services;

namespace system_backend.Repository
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private IAuthServices _authServices;
        private readonly UserManager<ApplicationUser> _userManager;
        public CompanyRepository(ApplicationDbContext db,
            IMapper mapper, IAuthServices authServices, UserManager<ApplicationUser> userManager) : base(db)
        {

            _db = db;
            _mapper = mapper;
            _authServices = authServices;
            _userManager = userManager;
        }
        public async Task<AuthModel> RegisterUserAsync(RegisterCompanyModel model)
        {
            //here i will make transaction
            var transaction = _db.Database.BeginTransaction();
            try
            {
                var companyModel = _mapper.Map<ApplicationUser>(model);
                var registerModel = _mapper.Map<RegisterModel>(model);
                var user = await _authServices.RegisterAsync(registerModel);
                if (!user.IsAuthenticated)
                {
                    return user;
                }
                model.Id = user.UserId;

                var company = _mapper.Map<Company>(model);
                
                // throw exectiopn to test roll back transaction
                //throw new Exception();

                await _db.Companies.AddAsync(company);
                _db.SaveChanges();
                transaction.Commit();

                return new AuthModel
                {
                    UserId = company.Id,
                    Username = user.Username,
                    ExpiresOn = new DateTime(),
                    IsAuthenticated = true,
                    Role = new List<string>() { model.Role},
                    Token = null,

                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                return new AuthModel { Message = ex.ToString() };

            }

        }
        public async Task<List<CompanyDTO>> GetCompaniesAsync()
        {

            var users = _userManager.Users;
            var companies = await (from user in _db.Users
                                   join userRole in _db.UserRoles
                                   on user.Id equals userRole.UserId
                                   join role in _db.Roles
                                   on userRole.RoleId equals role.Id
                                   where role.Name == "Company"
                                   join company in _db.Companies
                                   on user.Id equals company.Id

                                   select new CompanyDTO
                                   {
                                       Id = company.Id,
                                       UserDisplayName = user.UserDisplayName,
                                       UserName = user.UserName,
                                       

                                   }
                                ).ToListAsync();

            var companiesDTO = _mapper.Map<List<CompanyDTO>>(companies);
            return companiesDTO;

        }
        public async Task<CompanyModel> GetCompanyAsync(string id)
        {

            //var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            var companyModel = await (from user in _db.Users
                                where user.Id == id
                                join company in _db.Companies
                                 on id equals company.Id
                                select new CompanyModel
                                {

                                    Id = user.Id,
                                    UserName = user.UserName,
                                    UserDisplayName = user.UserDisplayName,
                                    Account = company.Account,
                                    Payments = 
                                    _mapper.Map<List<PaymentsDTO>>
                                    (_db.CompanyPayments.Where(i=>i.CompanyId == id)
                                    .OrderByDescending(i => i.Id).Take(10).ToList()),

                                    Bills = _mapper.Map<List<BillsDTO>>
                                    (_db.Bills.Where(i => i.CompanyId == id)
                                    .OrderByDescending(i => i.Id).Take(10).ToList()),
                                }).FirstOrDefaultAsync();

            var companyDTO = _mapper.Map<CompanyModel>(companyModel);
            return companyDTO;

        }
        public async Task<CompanyUpdateModel> UpdateAsync(CompanyUpdateModel companyDTO)
        {

            var company = await _userManager.FindByIdAsync(companyDTO.Id);
            company.UserName = companyDTO.UserName;
            company.UserDisplayName = companyDTO.UserDisplayName;
            await _userManager.UpdateAsync(company);
            await _db.SaveChangesAsync();
            return companyDTO;

        }

        public async Task DeleteCompanyAsync(string id)
        {
            var transaction = _db.Database.BeginTransaction();
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                await _userManager.DeleteAsync(user);
                var company = await _db.Companies.FindAsync(id);
                _db.Companies.Remove(company);
                await _db.SaveChangesAsync();
                transaction.Commit();
            }
            catch(Exception ex)
            {
                transaction.Rollback();
            }
        }
    }
}
