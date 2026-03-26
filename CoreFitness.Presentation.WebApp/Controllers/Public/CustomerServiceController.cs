using CoreFitness.Application.CustomerService.ContatRequests;
using CoreFitness.Application.CustomerService.ContatRequests.Inputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Presentation.WebApp.Models.CustomerService;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Public;


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

        if (result.IsFailure)
        {
            TempData["ErrorMessage"] = result.Error?.Message
            ?? "Your message could not be sent. Please try again later.";
        }
        else
        {
            TempData["SuccessMessage"] = "Your message has been sent.";
        }
        
        // alltid Get-versionen av Index
        return RedirectToAction(nameof(Index));
    }

}



