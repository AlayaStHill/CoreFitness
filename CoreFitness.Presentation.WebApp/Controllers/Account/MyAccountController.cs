using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Account;

public class MyAccountController : Controller
{
    public IActionResult AboutMe()
    {
        return View();
    }
}
