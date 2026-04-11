using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Admin;

[Route("admin/workoutsessions")]
[Authorize(Roles = "Admin")]
public class AdminWorkoutSessionsController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}

//[HttpPost("{id}/delete")]



