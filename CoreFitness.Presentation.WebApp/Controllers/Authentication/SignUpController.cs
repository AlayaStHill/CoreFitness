using CoreFitness.Application.Abstractions.Authentication;
using CoreFitness.Application.Abstractions.Authentication.Inputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Presentation.WebApp.Filters;
using CoreFitness.Presentation.WebApp.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Authentication;

public class SignUpController(IAuthService identityAuthService) : Controller
{
    // key (behållaren/identifiraren) som används i session och innehåller email (value/datan)
    private const string SignUpEmailSessionKey = "SignUpEmailSessionKey";
    private const string ReturnUrlSessionKey = "ReturnUrl";

    // visa sign-up vyn
    [HttpGet("sign-up")]
    [AllowAnonymous]
    [RedirectIfAuthenticated]
    public IActionResult SignUp(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }

    // spara email i session så att den kan användas i nästa steg (set-password) 
    [HttpPost("sign-up")]
    // för Cross-Site Request Forgery, en attacker skickaren post-request från en annan sida. ValidateAntiForgeryToken --> krävs att en giltig anti-forgery token skickas med i requesten
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public IActionResult SignUp(SignUpRequest request, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        HttpContext.Session.SetString(SignUpEmailSessionKey, request.Email);

        // IsLocalUrl används för att kontrollera att returnUrl är en lokal URL, vilket är viktigt för att förhindra open redirect-attacker. Om returnUrl inte är en lokal URL, kommer den inte att sparas i sessionen och användaren kommer att omdirigeras till set-password-sidan utan en returnUrl.
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            HttpContext.Session.SetString("ReturnUrl", returnUrl);
        }


        return RedirectToAction(nameof(SetPassword));
    }

    // hämta email från session och visa set-password vyn
    [HttpGet("set-password")]
    [AllowAnonymous]
    public IActionResult SetPassword()
    {
        string? email = HttpContext.Session.GetString(SignUpEmailSessionKey);
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(SignUp));

        return View(new SetPasswordRequest());
    }

    [HttpPost("set-password")]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> SetPassword(SetPasswordRequest request, CancellationToken ct = default)
    {
        string? email = HttpContext.Session.GetString(SignUpEmailSessionKey);
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(SignUp));

        if (!ModelState.IsValid)
            return View(request);

        SignUpUserInput input = new
        (
            email,
            request.Password
        );

        Result result = await identityAuthService.SignUpUserAsync(input, ct);

        if (result.IsFailure)
        {
            //string.Empty används för att lägga till ett globalt fel, inte kopplat till ett specifikt fält
            ModelState.AddModelError(string.Empty, result.Error?.Message
                ?? "An error occurred while signing up.");

            return View(request);
        }

        string? returnUrl = HttpContext.Session.GetString("ReturnUrl");

        TempData["SuccessMessage"] = "Your account has been created successfully. Please sign in.";

        HttpContext.Session.Remove(SignUpEmailSessionKey);
        HttpContext.Session.Remove("ReturnUrl");

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Account");
    }
}
