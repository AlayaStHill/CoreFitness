using CoreFitness.Domain.Aggregates.WorkoutCategories;
using CoreFitness.Domain.Aggregates.WorkoutTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Presentation.WebApp.Models.Admin;

public class CreateWorkoutSessionRequest
{
    [Required(ErrorMessage = "Workout type is required.")]
    public Guid WorkoutTypeId { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    public DateOnly Date { get; set; }

    [Required(ErrorMessage = "Start time is required.")]
    public TimeOnly StartTime { get; set; }

    [Required(ErrorMessage = "Duration is required.")]
    public TimeSpan Duration { get; set; }

    [Range(1, 40, ErrorMessage = "Capacity must be between {1} and {2}.")]
    public int Capacity { get; set; }

    public IEnumerable<SelectListItem> WorkoutTypes { get; set; } = [];
}

