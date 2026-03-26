using CoreFitness.Application.Abstractions.Authentication;
using CoreFitness.Application.Abstractions.Authentication.Inputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Presentation.WebApp.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Athentication;

public class AuthenticationController(IAuthService identityAuthService) : Controller
{
    [HttpGet("sign-up")]
    [AllowAnonymous]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost("sign-up")]
    [ValidateAntiForgeryToken]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp(SignUpRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        SignUpUserInput input = new
        (
            request.Email,
            request.Password
        );

        Result result = await identityAuthService.SignUpUserAsync(input);

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error?.Message ?? "An error occurred while signing up.");
            return View(request);
        }

        TempData["SuccessMessage"] = "Account created. Please sign in.";

        return RedirectToAction(nameof(SignIn));
    }
          
}
