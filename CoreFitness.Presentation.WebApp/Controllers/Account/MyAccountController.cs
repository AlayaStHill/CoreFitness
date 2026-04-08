using CoreFitness.Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Account;

[Authorize]
public class MyAccountController(ICurrentUserService currentUserService) : Controller
{
    public IActionResult AboutMe()
    {
        if (string.IsNullOrWhiteSpace(currentUserService.UserId))
            return Challenge();

        return View();
    }
}
