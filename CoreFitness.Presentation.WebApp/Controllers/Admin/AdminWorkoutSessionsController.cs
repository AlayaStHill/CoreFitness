using CoreFitness.Application.Admin.WorkoutSessions;
using CoreFitness.Application.Admin.WorkoutSessions.Outputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Presentation.WebApp.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreFitness.Presentation.WebApp.Controllers.Admin;

[Route("admin/workoutsessions")]
[Authorize(Roles = "Admin")]
public class AdminWorkoutSessionsController(IWorkoutSessionService sessionService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        Result<IReadOnlyList<WorkoutSessionOutput>> outputs = await sessionService.GetAllWorkoutSessionsAsync();

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
}

//[HttpPost("{id}/delete")]



