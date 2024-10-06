using CoursesWebsite.Areas.User.Data;
using CoursesWebsite.Models;
using Microsoft.AspNetCore.Identity;

namespace CoursesWebsite.Areas.Identity.Data
{
    public static class DbInitializer
    {
        public static async Task Initializer(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "User", "Instructor" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // create a default Admin user
            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "admin123",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };

                string password = "123456";
                var createAdmin = await userManager.CreateAsync(newAdmin, password);
                if (createAdmin.Succeeded) 
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
