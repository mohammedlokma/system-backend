using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
            modelBuilder.Entity<Safe>()
                .Property(b => b.Total)
                .HasDefaultValue(5);
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




    }

}
