using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using system_backend.Models;

namespace system_backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Agent> Agents { get; set; }

        public DbSet<ServicePlaces> ServicePlaces { get; set; }

        public DbSet<ExpensesPayments> Expenses { get; set; }
        public DbSet<CouponsPayments> Coupons { get; set; }


    }

}
