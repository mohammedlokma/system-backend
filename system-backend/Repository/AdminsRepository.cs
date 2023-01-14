using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using system_backend.Data;
using system_backend.Models;
using system_backend.Models.Dtos;
using system_backend.Repository.Interfaces;

namespace system_backend.Repository
{
    public class AdminsRepository : BaseRepository<ApplicationUser>,IAdminsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public AdminsRepository(ApplicationDbContext db, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IMapper mapper) :base(db)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<List<AdminDTO>> GetAdmins()
        {

            var users = _userManager.Users;
            IEnumerable<ApplicationUser> admins = await (from user in _db.Users
                          join userRole in _db.UserRoles
                          on user.Id equals userRole.UserId
                          join role in _db.Roles
                          on userRole.RoleId equals role.Id
                          where role.Name == "Admin"
                          select user)
                   .ToListAsync();
            var adminsDto = _mapper.Map<List<AdminDTO>>(admins);
            return adminsDto;

        }
    }
}
