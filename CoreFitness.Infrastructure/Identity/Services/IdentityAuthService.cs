using CoreFitness.Application.Abstractions.Authentication;
using CoreFitness.Application.Abstractions.Authentication.Inputs;
using CoreFitness.Application.Members;
using CoreFitness.Application.Shared;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CoreFitness.Infrastructure.Identity.Services;

public sealed class IdentityAuthService(IMemberRepository memberRepository, IUnitOfWork iUnitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IAuthService
{
    public async Task<Result> SignInExternalUserAsync(string? roleName)
    {
        ExternalLoginInfo? externalLoginInfo = await signInManager.GetExternalLoginInfoAsync();

        if (externalLoginInfo is null)
            return Result.Fail(ErrorTypes.Error, IdentityAuthErrors.ExternalLoginInfoMissing);

        // Kollar om en user finns kopplad till den externa login info (GitHub-kontot)
        SignInResult signInResult = await signInManager.ExternalLoginSignInAsync(
            externalLoginInfo.LoginProvider,
            externalLoginInfo.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true
        );

        if (signInResult.Succeeded)
            return Result.Success();

        // hämta data från den externa login info (GitHub) för att skapa en ny user
        string? email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
            return Result.Fail(ErrorTypes.BadRequest, IdentityAuthErrors.EmailIsRequired);

        ApplicationUser? user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            string? firstName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.GivenName);
            string? lastName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Surname);
            string imageUrl = externalLoginInfo.Principal.FindFirstValue("urn:github:avatar")
                ?? "~/images/profile-image-avatar.webp";

            user = ApplicationUser.Create(email);
            user.FirstName = firstName;
            user.LastName = lastName;
            user.ImageUrl = imageUrl;
            user.EmailConfirmed = true;

            IdentityResult createdResult = await userManager.CreateAsync(user);
            if (!createdResult.Succeeded)
            {
                string errorMessage = string.Join(", ", createdResult.Errors.Select(error => error.Description));
                return Result.Fail(ErrorTypes.Error, errorMessage);
            }

            if (!string.IsNullOrWhiteSpace(roleName))
            {
                IdentityResult roleResult = await userManager.AddToRoleAsync(user, roleName);
                if (!roleResult.Succeeded)
                {
                    return Result.Fail(ErrorTypes.Error, string.Join(", ", roleResult.Errors.Select(error => error.Description)));
                }
            }
        }

        // Kopplar Github-kontot (extern login info) till den nya usern
        IdentityResult loginResult = await userManager.AddLoginAsync(user, externalLoginInfo);
        // Om login misslyckas och inget av felen är LoginAlreadyAssociated (det förväntade), returnera error.
        if (!loginResult.Succeeded && loginResult.Errors.All(error => error.Code != "LoginAlreadyAssociated"))
        {
            string errorMessage = string.Join(", ", loginResult.Errors.Select(error => error.Description));
            return Result.Fail(ErrorTypes.Error, errorMessage);
        }

        await signInManager.SignInAsync(user, isPersistent: false);
        return Result.Success();
    }

    public async Task<Result> SignInUserAsync(SignInUserInput request)
    {
        if (request is null || string.IsNullOrWhiteSpace(request?.Email) || string.IsNullOrWhiteSpace(request?.Password))
            return Result.Fail(ErrorTypes.BadRequest, IdentityAuthErrors.InvalidCredentials);

        SignInResult signInResult = await signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, lockoutOnFailure: false);
        if (signInResult.IsLockedOut)
            return Result.Fail(ErrorTypes.Error, IdentityAuthErrors.UserLockedOut);

        if (signInResult.IsNotAllowed)
            return Result.Fail(ErrorTypes.Error, IdentityAuthErrors.UserNotAllowed);

        if (signInResult.RequiresTwoFactor)
            return Result.Fail(ErrorTypes.Error, IdentityAuthErrors.TwoFactorRequired);

        if (!signInResult.Succeeded)
            return Result.Fail(ErrorTypes.BadRequest, IdentityAuthErrors.InvalidCredentials);

        return Result.Success();
    }

    public async Task<Result> SignUpUserAsync(SignUpUserInput input, CancellationToken ct)
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

        IdentityResult createdResult = await userManager.CreateAsync(user, input.Password);
        if (!createdResult.Succeeded)
        {
            string errorMessage = string.Join(", ", createdResult.Errors.Select(error => error.Description));
            return Result.Fail(ErrorTypes.BadRequest, errorMessage);
        }

        Member member = Member.Create(user.Id);
        await memberRepository.AddAsync(member);
        await iUnitOfWork.SaveChangesAsync(ct);

        const string roleName = "Member";

        IdentityResult  roleResult = await userManager.AddToRoleAsync(user, roleName);
        if (!roleResult.Succeeded)
        {
            return Result.Fail(ErrorTypes.Error, string.Join(", ", roleResult.Errors.Select(error => error.Description)));
        }

        return Result.Success();

    }

    public Task SignOutUserAsync() => signInManager.SignOutAsync();
}
