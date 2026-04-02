using CoreFitness.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity.Data;
// Seedar
internal static class IdentityInitializer
{
    private const string AdminEmail = "admin@domain.com";
    private const string AdminPassword = "Password123!";
    private const string AdminRole = "Admin";

    // behandlas som konstant PascalCase
    private static readonly string[] DefaultRoles = { "Admin", "Member" };

    public static async Task InitializeDefaultRolesAsync(IServiceProvider serviceProvider)
    { 
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
        RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        //try catch? lägga till exception???
        foreach (string role in DefaultRoles)
        {
            if (await roleManager.RoleExistsAsync(role))
                continue;

            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    public static async Task InitializeDefaultAdminAccountsAsync(IServiceProvider serviceProvider)
    {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();

        UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        ApplicationUser? existingUser = await userManager.FindByEmailAsync(AdminEmail); 
        if (existingUser is not null)
            return;

        // denna istället för nedan??????
        ApplicationUser user = ApplicationUser.Create(AdminEmail);
        user.EmailConfirmed = true;

        IdentityResult result = await userManager.CreateAsync(user, AdminPassword);
        if (!result.Succeeded)
            return;

        if (await roleManager.RoleExistsAsync(AdminRole))
        {
            await userManager.AddToRoleAsync(user, AdminRole);
        }
    }
}
