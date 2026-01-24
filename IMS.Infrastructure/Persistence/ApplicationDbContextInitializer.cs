using IMS.Domain.Common.Enums;
using IMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.Persistence
{
    public class ApplicationDbContextInitializer(
        ApplicationDbContext context, 
        UserManager<User> userManager)
    {
        public async Task InitializeAsync()
        {
            try
            {
                if (context.Database.IsSqlServer())
                {
                    await context.Database.MigrateAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task SeedAsync()
        {
            var superAdminEmail = "super-admin@gmail.com";
            var superAdmin = new User
            {
                UserName = superAdminEmail,
                FirstName = "Super",
                LastName = "Admin",
                Email = superAdminEmail,
                EmailConfirmed = true,
                Role = Role.SuperAdmin
            };

            if (userManager.Users.All(u => u.UserName != superAdmin.UserName))
            {
                var result = await userManager.CreateAsync(superAdmin, "SuperAdmin123!");

                if (result.Succeeded)
                {
                    Console.WriteLine("SuperAdmin user seeded successfully.");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    Console.WriteLine("Failed to seed SuperAdmin: {Errors}", errors);
                }
            }

            if (!await context.InternetPlans.AnyAsync())
            {
                var plans = new List<InternetPlan>
                {
                    new InternetPlan
                    {
                        Name = "Fiber Starter",
                        Description = "Perfect for small households and basic browsing.",
                        SpeedMbps = 150,
                        Price = 1299.00m,
                        Currency = "PHP"
                    },
                    new InternetPlan
                    {
                        Name = "Fiber Pro Max",
                        Description = "High-speed connection for gaming and 4K streaming.",
                        SpeedMbps = 400,
                        Price = 2499.00m,
                        Currency = "PHP"
                    }
                };

                await context.InternetPlans.AddRangeAsync(plans);
                await context.SaveChangesAsync();
            }
        }
    }
}
