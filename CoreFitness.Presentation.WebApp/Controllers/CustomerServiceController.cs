using CoreFitness.Presentation.WebApp.Models.Forms;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers;


public class CustomerServiceController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new ContactForm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(ContactForm form, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return View(form);

        TempData["ContactFormMessage"] = "Your message has been sent.";
        return RedirectToAction(nameof(Index));
    }

}



