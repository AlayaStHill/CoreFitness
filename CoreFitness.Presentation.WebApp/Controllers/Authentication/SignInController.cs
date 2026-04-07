using CoreFitness.Application.Abstractions.Authentication;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Infrastructure.Identity.Models;
using CoreFitness.Presentation.WebApp.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Authentication;

public class SignInController(IAuthService identityAuthService, SignInManager<ApplicationUser> signInManager) : Controller
{
    [HttpGet("sign-in")]
    [AllowAnonymous]
    [RedirectIfAuthenticated]
    public IActionResult SignIn(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        return View(); 
    }



    //ModelState.AddModelError(string.Empty, "Incorrect email or password");


    // Redirectar användaren till GitHub.
    [HttpPost("external-login")]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public IActionResult ExternalLogin(string provider, string? returnUrl = null)
    {
        string callbackUrl = Url.Action(nameof(ExternalLoginCallback), "SignIn", new { returnUrl })!;

        // sparar returnUrl, sätter redirectUrl till callback URL, och skickar användaren till den externa leverantören för autentisering
        AuthenticationProperties properties = signInManager.ConfigureExternalAuthenticationProperties(provider, callbackUrl);

        // en redirect som går till den externa leverantören, GitHub, för att autentisera användaren, och sedan tillbaka till vår callback URL
        return Challenge(properties, provider);
    }

    // Hanterar återkomsten från extern leverantör (GitHub), kontrollerar fel, loggar in eller skapar användaren och redirectar vidare.
    [HttpGet]
    [AllowAnonymous]
    [RedirectIfAuthenticated]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl, string? remoteError = null)
    {
        //fel från GitHub
        if (!string.IsNullOrWhiteSpace(remoteError))
        {
            TempData["ErrorMessage"] = $"External provider error: {remoteError}";
            return RedirectToAction(nameof(SignIn), new { returnUrl });
        }

        Result signInResult = await identityAuthService.SignInExternalUserAsync("Member");
        if (signInResult.IsFailure)
        {
            TempData["ErrorMessage"] = signInResult.Error;
            return RedirectToAction(nameof(SignIn), new { returnUrl });
        }

        if (!string.IsNullOrWhiteSpace(returnUrl))
            return Redirect(returnUrl);

        // mappar till default route
        return Redirect("/");
    }
}
