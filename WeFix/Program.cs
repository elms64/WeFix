using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeFix.Data;
using WeFix.Models;
using WeFix.Areas.Identity.Data;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using WeFix.Authorization;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        //Defining context for users and user roles
        builder.Services.AddDefaultIdentity<ApplicationUser>
            (options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<ApplicationDbContext>();



        builder.Services.AddRazorPages();

        //Sets the fallback policy so that only authorised users can access resources unless explicitly defined with the [AllowAnonymous] attribute
        //  builder.Services.AddControllers(config =>
        //   {
        //        var policy = new AuthorizationPolicyBuilder()
        //                        .RequireAuthenticatedUser()
        //                        .Build();
        //        config.Filters.Add(new AuthorizeFilter(policy));
        ///   });

        builder.Services.AddScoped<IAuthorizationHandler,
                      ContactIsOwnerAuthorizationHandler>();

        builder.Services.AddSingleton<IAuthorizationHandler,
                              ContactAdministratorsAuthorizationHandler>();

        builder.Services.AddSingleton<IAuthorizationHandler,
                              ContactManagerAuthorizationHandler>();
        builder.Services.Configure<IdentityOptions>(options =>

        {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            options.SlidingExpiration = true;
        });



        var app = builder.Build();


        //Automatically create some authorisation roles for the system 
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "SysAdmin", "Manager", "Technician", "Reception", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            // requires using Microsoft.Extensions.Configuration;
            // Set password with the Secret Manager tool.
            // dotnet user-secrets set SeedUserPW <pw>

            var testUserPw = builder.Configuration.GetValue<string>("SeedUserPW");

            await SeedData.Initialize(services, testUserPw);

        }

        //Automatically create an Admin account with full access (password can be changed later)
        /*   using (var scope = app.Services.CreateScope())
           {
               var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

               string email = "admin@admin.com";
               string password = "Test1234,";

               if (await userManager.FindByEmailAsync(email) == null)
               {
                   var user = new ApplicationUser
                   {
                       FirstName = "System Admin",
                       Surname = "Admin",
                       UserName = "SysAdmin",
                       Email = email,
                       EmailConfirmed = true
                   };

                   await userManager.CreateAsync(user, password);

                   await userManager.AddToRoleAsync(user, "SysAdmin");
               }


           }
        */
        //Seed some parts data into the database from the 'SeedData' class
        /// using (var scope = app.Services.CreateScope())
        // {
        //     var services = scope.ServiceProvider;

        //     SeedData.Initialize(services);
        //
        //   }


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}