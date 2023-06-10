using WeFix.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WeFix.Models;
using Microsoft.AspNetCore.Identity;

namespace WeFix.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Identity");
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }

        public DbSet<Part> Part { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<CompletedAppointment> CompletedAppointment { get; set; }
        public DbSet<AppointmentPartsUsed> AppointmentPartsUsed { get; set; }
    }
}
