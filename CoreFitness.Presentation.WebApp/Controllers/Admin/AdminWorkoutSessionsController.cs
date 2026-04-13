using CoreFitness.Application.Admin.WorkoutSessions;
using CoreFitness.Application.Admin.WorkoutSessions.Outputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Presentation.WebApp.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Admin;

[Route("admin/workoutsessions")]
[Authorize(Roles = "Admin")]
public class AdminWorkoutSessionsController(IWorkoutSessionService sessionService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        Result<IReadOnlyList<WorkoutSessionOutput>> outputs = await sessionService.GetAllWorkoutSessionsAsync(ct);

        var viewModel = new AdminWorkoutSessionsViewModel
        {
            Sessions = outputs.Value.Select(session => new WorkoutSessionItemViewModel
            {
                Id = session.Id,
                Category = session.Category,
                Type = session.Type,
                StartsAt = session.StartsAt,
                Duration = session.Duration,
                Capacity = session.Capacity
            }).ToList()
        };

        return View(viewModel);
    }

    //Klick -> HTMX-attribut -> request -> server -> HTML tillbaka -> Controller → return "" → DOM uppdateras - raden tas bort 
    [HttpPost("delete/{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
       await sessionService.DeleteWorkoutSessionAsync(new WorkoutSessionId(id), ct);

        // returnerar tomt innehåll, som HTMX ersätter den raden med, så den försvinner från listan
        return Content("");  
    }

}



