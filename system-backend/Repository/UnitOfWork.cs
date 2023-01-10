using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using system_backend.Data;
using system_backend.Models;
using system_backend.Models.Dtos;
using system_backend.Repository.Interfaces;

namespace system_backend.Repository
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public UnitOfWork(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            Admins = new AdminsRepository(_db,_userManager,
            _roleManager, _mapper);
        }
        public IAdminsRepository Admins { get; private set; }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }


        public void Dispose()
        {
            _db.Dispose();
        }

    }
}
