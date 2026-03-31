using CoreFitness.Application.Abstractions.Authentication;
using CoreFitness.Application.Abstractions.Authentication.Inputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace CoreFitness.Infrastructure.Identity.Services;

public sealed class IdentityAuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager) : IAuthService
{
    public async Task<Result> SignUpUserAsync(SignUpUserInput input, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(input.Email))
            return Result.Fail(ErrorTypes.BadRequest, IdentityAuthErrors.EmailIsRequired);

        if (string.IsNullOrWhiteSpace(input.Password))
            return Result.Fail(ErrorTypes.BadRequest, IdentityAuthErrors.PasswordIsRequired);

        ApplicationUser? existingUser = await userManager.FindByEmailAsync(input.Email);
        if (existingUser is not null)
            return Result.Fail(ErrorTypes.Conflict, IdentityAuthErrors.UserAlreadyExists);

        ApplicationUser user = new()         
        {
            UserName = input.Email,
            Email = input.Email
        };

        IdentityResult created = await userManager.CreateAsync(user, input.Password);
        if (!created.Succeeded)
        {
            string[] errors = created.Errors.Select(error => error.Description).ToArray();
            return Result.Fail(ErrorTypes.BadRequest, string.Join(", ", errors));
        }

        const string roleName = "Member";

        IdentityResult  roleResult = await userManager.AddToRoleAsync(user, roleName);
        if (!roleResult.Succeeded)
        {
            return Result.Fail(ErrorTypes.Error, string.Join(", ", roleResult.Errors.Select(error => error.Description)));
        }

        return Result.Success();

    }


}
