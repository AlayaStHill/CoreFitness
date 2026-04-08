using CoreFitness.Application.Abstractions.Authentication;
using CoreFitness.Application.MyAccount;
using CoreFitness.Application.MyAccount.Inputs;
using CoreFitness.Presentation.WebApp.Models.MyAccount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Account;

[Authorize]
public class MyAccountController(
    ICurrentUserService currentUserService,
    IMyAccountUserService myAccountUserService,
    IProfileImageStorageService profileImageStorageService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> AboutMe()
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        var user = await myAccountUserService.GetByIdAsync(currentUserService.UserId);
        if (user is null)
            return Challenge();

        AboutMeFormModel model = new()
        {
            FirstName = user.FirstName ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            ImageUrl = user.ImageUrl
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AboutMe(AboutMeFormModel model, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        if (!ModelState.IsValid)
            return View(model);

        string? imageUrl = model.ImageUrl;

        if (model.ProfileImage is not null && model.ProfileImage.Length > 0)
        {
            await using Stream stream = model.ProfileImage.OpenReadStream();
            imageUrl = await profileImageStorageService.SaveProfileImageAsync(stream, model.ProfileImage.FileName, ct);
        }

        var input = new UpdateMyAccountUserInput(
            currentUserService.UserId,
            model.Email,
            model.FirstName,
            model.LastName,
            model.PhoneNumber,
            imageUrl);

        var result = await myAccountUserService.UpdateAsync(input);
        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error?.Message ?? "Could not update profile.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Profile updated successfully.";
        return RedirectToAction(nameof(AboutMe));
    }
}
