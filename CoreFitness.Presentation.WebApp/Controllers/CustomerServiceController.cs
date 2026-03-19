using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers;

public class CustomerServiceController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
