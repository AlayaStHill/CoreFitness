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
    // key (namnet) som används i session och innehåller email (value)
    private const string SignUpEmailSessionKey = "SignUpEmailSessionKey";

    [HttpGet("sign-up")]
    [AllowAnonymous]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost("sign-up")]
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

    [HttpGet("set-password")]
    public IActionResult SetPassword()
    {
        string? email = HttpContext.Session.GetString(SignUpEmailSessionKey);
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToAction(nameof(SignUp));

        return View();
    }

    //[HttpPost("set-password")]
    //[Authorize]
    //public async Task<IActionResult> SetPassword(/*SetPasswordRequest request*/)
    //{

    //    //SignUpUserInput input = new
    //    //(
    //    //    request.Email,
    //    //    request.Password
    //    //);

    //    return View();
    //}

}



//        Result result = await identityAuthService.SignUpUserAsync(input);

//        if (result.IsFailure)
//        {
//            ModelState.AddModelError(string.Empty, result.Error?.Message ?? "An error occurred while signing up.");
//            return View(request);
//        }

//        TempData["SuccessMessage"] = "Account created. Please sign in.";

//return RedirectToAction(nameof(SignIn));