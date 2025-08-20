using GiderTakipSistemi.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GiderTakipSistemi.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAndAdminAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Roller
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Varsayılan admin
            var adminEmail = "admin@gider.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    AdSoyad = "Admin Kullanıcı"
                };
                await userManager.CreateAsync(adminUser, "Admin123!"); // Şifre
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
