using Microsoft.EntityFrameworkCore;
using WeFix.Data;

namespace WeFix.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<ApplicationDbContext>>()))
        {
            if (context == null || context.Part == null)
            {
                throw new ArgumentNullException("Null RazorPagesMovieContext");
            }

            // Look for any parts.
            if (context.Part.Any())
            {
                return;   // DB has been seeded
            }

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
            context.SaveChanges();
        }
    }
}