using WeFix.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WeFix.Models;


namespace WeFix.Data;

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

    public DbSet<WeFix.Models.Part> Part { get; set; } = default!;

    public DbSet<WeFix.Models.Appointment> Appointment { get; set; } = default!;

    public DbSet<WeFix.Models.Customer> Customer { get; set; } = default!;

    public DbSet<WeFix.Models.Employee> Employee { get; set; } = default!;


}



