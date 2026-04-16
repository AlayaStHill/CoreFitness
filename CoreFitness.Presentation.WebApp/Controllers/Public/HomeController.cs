using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Public;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
