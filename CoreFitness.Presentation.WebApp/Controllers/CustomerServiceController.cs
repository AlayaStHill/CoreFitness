using CoreFitness.Presentation.WebApp.Models.Forms;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers;


public class CustomerServiceController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(ContactForm form)
    {
        return View();
    }

}



