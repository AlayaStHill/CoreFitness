using Azure.Core;
using CoreFitness.Application.Abstractions.Authentication;
using CoreFitness.Application.Abstractions.Authentication.Inputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Presentation.WebApp.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Athentication;

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
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public IActionResult SignUp(SignUpRequest signUpRequest)
    {
        if (!ModelState.IsValid)
        {
            return View(signUpRequest);
        }

        HttpContext.Session.SetString(SignUpEmailSessionKey, signUpRequest.Email);

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

        return View();
    }

    [HttpPost("set-password")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> SetPassword(SetPasswordRequest passwordRequest)
    {
        string? email = HttpContext.Session.GetString(SignUpEmailSessionKey);
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(SignUp));

        if (!ModelState.IsValid)
            return View(passwordRequest);

        SignUpUserInput input = new
        (
            email,
            passwordRequest.Password
        );

        return View();
    }

}



//        Result result = await identityAuthService.SignUpUserAsync(input);

//        if (result.IsFailure)
//        {
//            ModelState.AddModelError(string.Empty, result.Error?.Message ?? "An error occurred while signing up.");
//            return View(request);
//        }

//        TempData["SuccessMessage"] = "Account created. Please sign in.";

//return RedirectToAction(nameof(SignIn));