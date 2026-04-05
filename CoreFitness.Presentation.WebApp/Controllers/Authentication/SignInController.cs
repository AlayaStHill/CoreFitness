using CoreFitness.Infrastructure.Identity.Models;
using CoreFitness.Presentation.WebApp.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Authentication;

public class SignInController(SignInManager<ApplicationUser> signInManager) : Controller
{
    [HttpGet("sign-in")]
    [AllowAnonymous]
    [RedirectIfAuthenticated]
    public IActionResult SignIn(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }


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

    [HttpGet]
    [AllowAnonymous]
    public IActionResult ExternalLoginCallback()
    {
        return View();
    }
}
