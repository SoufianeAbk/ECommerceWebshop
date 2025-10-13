using Microsoft.AspNetCore.Identity;
using ECommerceWebshop.Models;

namespace ECommerceWebshop.Data
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Seed Roles
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Seed Admin User
            var adminEmail = "admin@shopbe.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "ShopBE",
                    Address = "Nieuwstraat 123",
                    City = "Brussel",
                    PostalCode = "1000",
                    PhoneNumber = "+32 48 123 4567",
                    RegistrationDate = DateTime.Now
                };

                var createAdmin = await userManager.CreateAsync(newAdmin, "Admin123!");

                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }

            // Seed Regular User
            var userEmail = "user@shopbe.com";
            var regularUser = await userManager.FindByEmailAsync(userEmail);

            if (regularUser == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true,
                    FirstName = "Jan",
                    LastName = "Janssens",
                    Address = "Hoofdstraat 45",
                    City = "Antwerpen",
                    PostalCode = "2000",
                    PhoneNumber = "+32 49 876 5432",
                    RegistrationDate = DateTime.Now
                };

                var createUser = await userManager.CreateAsync(newUser, "User123!");

                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, "User");
                }
            }
        }
    }
}