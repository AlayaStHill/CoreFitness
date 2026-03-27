using Azure.Core;
using CoreFitness.Application.Abstractions.Authentication;
using CoreFitness.Application.Abstractions.Authentication.Inputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Presentation.WebApp.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Authentication;

public class AuthenticationController(IAuthService identityAuthService) : Controller
{
    // key (behållaren/identifiraren) som används i session och innehåller email (value/datan)
    private const string SignUpEmailSessionKey = "SignUpEmailSessionKey";

    // visa sign-up vyn
    [HttpGet("sign-up")]
    [AllowAnonymous]
    public IActionResult SignUp()
    {
        return View();
    }

    // spara email i session så att den kan användas i nästa steg (set-password) 
    [HttpPost("sign-up")]
    // för Cross-Site Request Forgery, en attacker skickaren post-request från en annan sida. ValidateAntiForgeryToken --> krävs att en giltig anti-forgery token skickas med i requesten
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public IActionResult SignUp(SignUpRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        HttpContext.Session.SetString(SignUpEmailSessionKey, request.Email);

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

        TempData["SuccessMessage"] = "Your account has been created successfully. Please sign in.";

        HttpContext.Session.Remove(SignUpEmailSessionKey);
        return RedirectToAction(nameof(SignIn));
    }

}