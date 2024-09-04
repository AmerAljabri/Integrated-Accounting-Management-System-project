using accountSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>(); // استخدام ApplicationUser

        string[] roleNames = { "Admin", "Basic" };
        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // تهيئة المستخدم الإداري (اختياري)
        var adminUser = new ApplicationUser // استخدام ApplicationUser
        {
            UserName = "admin@domain.com",
            Email = "admin@domain.com"
        };

        string adminPassword = "Admin@123";
        var user = await userManager.FindByEmailAsync(adminUser.Email);

        if (user == null)
        {
            var createAdmin = await userManager.CreateAsync(adminUser, adminPassword);
            if (createAdmin.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}