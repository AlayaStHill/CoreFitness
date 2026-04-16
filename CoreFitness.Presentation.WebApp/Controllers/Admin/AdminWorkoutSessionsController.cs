using CoreFitness.Application.Admin.WorkoutSessions;
using CoreFitness.Application.Admin.WorkoutSessions.Inputs;
using CoreFitness.Application.Admin.WorkoutSessions.Outputs;
using CoreFitness.Application.Admin.WorkoutTypes;
using CoreFitness.Application.Admin.WorkoutTypes.Outputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Domain.Aggregates.WorkoutTypes;
using CoreFitness.Presentation.WebApp.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoreFitness.Presentation.WebApp.Controllers.Admin;

[Route("admin/workoutsessions")]
[Authorize(Roles = "Admin")]
public class AdminWorkoutSessionsController(IWorkoutSessionService sessionService, IWorkoutTypeService workoutTypeService) : Controller
{
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        Result<IReadOnlyList<WorkoutSessionOutput>> sessionsResult = await sessionService.GetAllWorkoutSessionsAsync(ct);

        string? error = sessionsResult.IsFailure
        ? sessionsResult.Error?.Message
        : null;

        // om det lyckas mappa till viewmodel annars tom lista
        var sessions = sessionsResult.IsSuccess
        ? sessionsResult.Value.Select(session => new WorkoutSessionItemViewModel
        {
            Id = session.Id,
            Category = session.Category,
            Type = session.Type,
            StartsAt = session.StartsAt,
            Duration = session.Duration,
            Capacity = session.Capacity
        }).ToList()
        : new List<WorkoutSessionItemViewModel>();

        // HTMX CHECK - Om requesten kommer från HTMX: returnera endast tabellen (HTML-fragment), partial view
        if (Request.Headers["HX-Request"] == "true")
        {
            //tabellen ska kunna visa fel
            ViewBag.Error = error;
            return PartialView("~/Views/Shared/Partials/Admin/_WorkoutSessionsTable.cshtml", sessions);
        }
        // annars returnera hela sidan, vanlig MVC-rendering
        var viewModel = new AdminWorkoutSessionsViewModel
        {
            Sessions = sessions
        };
        // skickas inte som parameter, finns implicit i viewn
        ViewBag.Error = error;
        return View(viewModel);
    }

    // Klick → HTMX POST → controller → success: return "" → <tr> ersätts → raden tas bort. Failure: return 400 → ingen DOM-ändring (raden är kvar)
    [HttpPost("delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
       Result result = await sessionService.DeleteWorkoutSessionAsync(new WorkoutSessionId(id), ct);

        if (result.IsFailure)
        {
            return StatusCode(400, result.Error?.Message ?? "Could not delete session");
        }

        // returnerar tomt innehåll, som HTMX ersätter den raden med, så den försvinner från listan
        return Content("");  
    }


    // förbereder formuläret med dropdown-listan
    [HttpGet("create-form")]
    public async Task<IActionResult> GetCreateForm(CancellationToken ct = default)
    {
        Result<IReadOnlyList<WorkoutTypeOutput>> workoutTypesResult = await workoutTypeService.GetWorkoutTypesAsync(ct);

        // // Förbereder modellen som används i create-formuläret (select-listan behöver workout types)
        var request = new CreateWorkoutSessionRequest();

        if (workoutTypesResult.IsFailure)
        {
            ModelState.AddModelError("", workoutTypesResult.Error?.Message ?? "Could not load workout types");
            // tom lista så select inte kraschar
            request.WorkoutTypes = new List<SelectListItem>();
        }

        request.WorkoutTypes = MapToSelectListItems(workoutTypesResult.Value);

        return PartialView("~/Views/Shared/Partials/Admin/_CreateWorkoutSessionForm.cshtml", request);
    }

    // För Cancel-knapp. Create form töms bort från DOM - ersätts med en tom sträng så att den inte är synlig längre
    [HttpGet("clear-form")]
    public IActionResult ClearCreateForm()
    {
        return Content(string.Empty);
    }


    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateWorkoutSessionRequest request, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            request.WorkoutTypes = await GetWorkoutTypeSelectListAsync(ct);
            return PartialView("~/Views/Shared/Partials/Admin/_CreateWorkoutSessionForm.cshtml", request);
        }

        // Date + StartTime = StartsAt domain modell
        DateTime dateTime = request.Date!.Value.ToDateTime(request.StartTime!.Value);
        DateTimeOffset startsAt = new DateTimeOffset(dateTime, DateTimeOffset.Now.Offset);

        var input = new CreateWorkoutSessionInput
        {
            WorkoutTypeId = new WorkoutTypeId(request.WorkoutTypeId!.Value),
            StartsAt = startsAt,
            Duration = request.Duration!.Value,
            Capacity = request.Capacity!.Value
        };

        Result result = await sessionService.CreateWorkoutSessionAsync(input, ct);

        if (result.IsFailure)
        {
            ModelState.AddModelError(string.Empty, result.Error?.Message ?? "An error occurred while creating the workout session.");

            request.WorkoutTypes = await GetWorkoutTypeSelectListAsync(ct);

            return PartialView("~/Views/Shared/Partials/Admin/_CreateWorkoutSessionForm.cshtml", request);
        }

        // triggar tabell reload
        Response.Headers["HX-Trigger-After-Swap"] = "session-created";

        ModelState.Clear();

        return PartialView("~/Views/Shared/Partials/Admin/_CreateWorkoutSessionForm.cshtml", new CreateWorkoutSessionRequest
        {
            WorkoutTypes = await GetWorkoutTypeSelectListAsync(ct)

        });
    }

    private static List<SelectListItem> MapToSelectListItems(IReadOnlyList<WorkoutTypeOutput> types)
    {
        return types.Select(wt => new SelectListItem
        {
            Value = wt.Id.ToString(),
            Text = $"{wt.Title} ({wt.WorkoutCategory})"
        }).ToList();
    }

    private async Task<List<SelectListItem>> GetWorkoutTypeSelectListAsync(CancellationToken ct)
    {
        Result<IReadOnlyList<WorkoutTypeOutput>> result = await workoutTypeService.GetWorkoutTypesAsync(ct);

        if (result.IsFailure)
            return new List<SelectListItem>();

        return MapToSelectListItems(result.Value);
    }
}

