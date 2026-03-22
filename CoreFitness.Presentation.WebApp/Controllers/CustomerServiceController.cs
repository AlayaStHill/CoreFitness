using CoreFitness.Application.CustomerService.ContatRequests;
using CoreFitness.Application.CustomerService.ContatRequests.Inputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Presentation.WebApp.Models.Forms;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers;


public class CustomerServiceController(IContactRequestService contactRequestService) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new ContactForm());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ContactForm form, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return View(form);

        ContactRequestInput contactRequestInput = new
            (
                form.FirstName,
                form.LastName,
                form.Email,
                form.PhoneNumber,
                form.Message
            );

        Result result = await contactRequestService.CreateContactRequestAsync(contactRequestInput, ct);

        TempData["ContactFormMessage"] = result.IsSuccess
            ? "Your message has been sent."
            : "Your message could not be sent. Please try again later.";

        return RedirectToAction(nameof(Index));
    }

}



