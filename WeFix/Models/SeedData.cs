using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeFix.Areas.Identity.Data;
using WeFix.Data;
using WeFix.Models;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
        {
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

            await CreateUserWithRole(userManager, "user1@example.com", "Test1234,", "John", "Doe", "User");
            await CreateUserWithRole(userManager, "reception1@example.com", "Test1234,", "Jane", "Doe", "Reception");
            await CreateUserWithRole(userManager, "technician1@example.com", "Test1234,", "Jack", "Doe", "Technician");
            await CreateUserWithRole(userManager, "manager1@example.com", "Test1234,", "Jan", "Doe", "Manager");
        }
    }

    private static async Task CreateUserWithRole(UserManager<ApplicationUser> userManager, string email, string password, string firstName, string surname, string roleName)
    {
        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser == null)
        {
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
        else
        {
            throw new Exception($"User '{email}' already exists.");
        }
    }
}
