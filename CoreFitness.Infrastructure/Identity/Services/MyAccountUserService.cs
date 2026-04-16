using CoreFitness.Application.MyAccount;
using CoreFitness.Application.MyAccount.Inputs;
using CoreFitness.Application.MyAccount.Outputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace CoreFitness.Infrastructure.Identity.Services;

public sealed class MyAccountUserService(UserManager<ApplicationUser> userManager) : IMyAccountUserService
{
    public async Task<MyAccountUserOutput?> GetByIdAsync(string userId)
    {
        ApplicationUser? user = await userManager.FindByIdAsync(userId);
        if (user is null) return null;

        return new MyAccountUserOutput(
            user.Id,
            user.Email ?? string.Empty,
            user.FirstName,
            user.LastName,
            user.PhoneNumber,
            user.ImageUrl
        );
    }

    public async Task<Result> UpdateAsync(UpdateMyAccountUserInput input)
    {
        ApplicationUser? user = await userManager.FindByIdAsync(input.UserId);
        if (user is null)
            return Result.Fail(MyAccountErrors.UserNotFound);

        ApplicationUser.UpdateDetails(
            user,
            input.FirstName,
            input.LastName,
            input.ImageUrl,
            input.PhoneNumber);

        user.Email = input.Email;
        user.UserName = input.Email;

        IdentityResult result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return Result.Fail(MyAccountErrors.IdentityUpdateFailed(string.Join(", ", result.Errors.Select(e => e.Description))));

        return Result.Success();
    }
}
