using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using system_backend.Models;

namespace system_backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Safe>().HasData(
            new Safe { Id=1,Total=0.0});
            SeedUsers(modelBuilder);
        }
        private static void SeedUsers(ModelBuilder builder)
        {
            string ADMIN_ID = "02174cf0–9412–4cfe - afbf - 59f706d72cf6";
            string ADMIN_ROLE_ID = Guid.NewGuid().ToString();
            string USER_ROLE_ID = Guid.NewGuid().ToString();
            string Company_ROLE_ID = Guid.NewGuid().ToString();
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = ADMIN_ROLE_ID,
                ConcurrencyStamp = ADMIN_ROLE_ID
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER",
                Id = USER_ROLE_ID,
                ConcurrencyStamp = USER_ROLE_ID
            });
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Name = "Company",
                NormalizedName = "Company",
                Id = Company_ROLE_ID,
                ConcurrencyStamp = Company_ROLE_ID
            });

            var user = new ApplicationUser()
            {
                Id = ADMIN_ID,
                UserName = "admin",
                UserDisplayName = "ADMIN",
                NormalizedUserName="ADMIN",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                LockoutEnabled = false,
                PhoneNumber = "1234567890"
            };

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, "Admin@123");

            builder.Entity<ApplicationUser>().HasData(user);

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ADMIN_ROLE_ID,
                UserId = ADMIN_ID
            });
        }

       
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Agent> Agents { get; set; }

        public DbSet<ServicePlaces> ServicePlaces { get; set; }
        public DbSet<AgentServicePlaces> AgentServicePlaces { get; set; }
        public DbSet<ExpensesPayments> Expenses { get; set; }
        public DbSet<CouponsPayments> Coupons { get; set; }
        public DbSet<Safe> Safe { get; set; }
        public DbSet<SafeInputs> SafeInputs { get; set; }
        public DbSet<SafeOutputs> SafeOutputs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Bills> Bills { get; set; }
        public DbSet<ReportItems> ReportItems { get; set; }
        public DbSet<CompanyReportItems> CompanyReportItems { get; set; }
        public DbSet<CompanyPayments> CompanyPayments { get; set; }
        public DbSet<BillDetails> BillDetails { get; set; }

        public DbSet<FullReport> FullReport { get; set; }




    }

}
