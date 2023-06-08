using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeFix.Areas.Identity.Data;
using WeFix.Data;
using WeFix.Models;
using WeFix.Authorization;
using Constants = WeFix.Authorization.Constants;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
            // For sample purposes seed both with the same password.
            // Password is set with the following:
            // dotnet user-secrets set SeedUserPW <pw>
            // The admin user can do anything

            // var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@contoso.com");
            //  await EnsureRole(serviceProvider, adminID, Constants.AppointmentAdministratorsRole);

            // allowed user can create and edit contacts that they create
            //  var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@contoso.com");
            //   await EnsureRole(serviceProvider, managerID, Constants.AppointmentManagersRole);


            if (context == null)
            {
                throw new ArgumentNullException("Null ApplicationDbContext");
            }

            // Add parts if not already added
            if (!context.Part.Any())
            {
                context.Part.AddRange(
                    new Part
                    {
                        PartName = "Sparco Steering Wheel",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        CarModel = "Universal",
                        Price = 87.99M,
                        StockQuantity = 12
                    },

                    new Part
                    {
                        PartName = "13inch Tyres",
                        ReleaseDate = DateTime.Parse("2021-1-27"),
                        CarModel = "Universal",
                        Price = 29.99M,
                        StockQuantity = 12
                    },

                    new Part
                    {
                        PartName = "Driver Side Rear Door",
                        ReleaseDate = DateTime.Parse("1999-3-10"),
                        CarModel = "Swift",
                        Price = 60.00M,
                        StockQuantity = 2
                    },

                    new Part
                    {
                        PartName = "Alternator",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        CarModel = "Carry Van",
                        Price = 284.59M,
                        StockQuantity = 4
                    }
                );
                await context.SaveChangesAsync();
            }



            // Create users with roles
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await CreateUserWithRole(userManager, "admin@example.com", testUserPw, "John", "Doe", "SysAdmin");
            await CreateUserWithRole(userManager, "user1@example.com", testUserPw, "John", "Doe", "User");
            await CreateUserWithRole(userManager, "reception1@example.com", testUserPw, "Jane", "Doe", "Reception");
            await CreateUserWithRole(userManager, "technician1@example.com", testUserPw, "Jack", "Doe", "Technician");
            await CreateUserWithRole(userManager, "manager1@example.com", testUserPw, "Jan", "Doe", "Manager");
        }
    }

    private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string UserName)
    {
        var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

        var user = await userManager.FindByNameAsync(UserName);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = UserName,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, testUserPw);
        }

        if (user == null)
        {
            throw new Exception("The password is probably not strong enough!");
        }

        return user.Id;
    }

    private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                  string uid, string role)
    {
        var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

        if (roleManager == null)
        {
            throw new Exception("roleManager null");
        }

        IdentityResult IR;
        if (!await roleManager.RoleExistsAsync(role))
        {
            IR = await roleManager.CreateAsync(new IdentityRole(role));
        }

        var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

        //if (userManager == null)
        //{
        //    throw new Exception("userManager is null");
        //}

        var user = await userManager.FindByIdAsync(uid);

        if (user == null)
        {
            throw new Exception("The testUserPw password was probably not strong enough!");
        }

        IR = await userManager.AddToRoleAsync(user, role);

        return IR;
    }

    private static async Task CreateUserWithRole(UserManager<ApplicationUser> userManager, string email, string password, string firstName, string surname, string roleName)
    {
        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            // User already exists, so just return without doing anything
            return;
        }

        var user = new ApplicationUser
        {
            FirstName = firstName,
            Surname = surname,
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, roleName);
        }
        else
        {
            var errors = result.Errors.Select(e => e.Description);
            throw new Exception($"Failed to create user '{email}'. Errors: {string.Join(", ", errors)}");
        }
    }

}
