using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Error;

public class ErrorController : Controller
{
    [Route("error/404")]
    public IActionResult NotFoundPage()
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        return View("NotFound");
    }
}
