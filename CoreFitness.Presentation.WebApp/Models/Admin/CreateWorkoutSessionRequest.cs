using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CoreFitness.Presentation.WebApp.Models.Admin;

public class CreateWorkoutSessionRequest : IValidatableObject
{
    [Required(ErrorMessage = "Workout type is required.")]
    public Guid? WorkoutTypeId { get; set; }

    [Required(ErrorMessage = "Date is required.")]
    public DateOnly? Date { get; set; }

    [Required(ErrorMessage = "Start time is required.")]
    public TimeOnly? StartTime { get; set; }

    [Required(ErrorMessage = "Duration is required.")]
    public TimeSpan? Duration { get; set; }

    [Required(ErrorMessage = "Capacity is required.")]
    [Range(1, 40, ErrorMessage = "Capacity must be between {1} and {2}.")]
    public int? Capacity { get; set; }

    // för att visa dropdown-lista över tillgängliga workouttypes <select>
    public IEnumerable<SelectListItem> WorkoutTypes { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Duration.HasValue && Duration.Value <= TimeSpan.Zero)
        {
            yield return new ValidationResult(
                "Duration must be greater than 00:00.",
                [nameof(Duration)]);
        }

        if (!Date.HasValue || !StartTime.HasValue)
        {
            yield break;
        }

        DateTime localDateTime = Date.Value.ToDateTime(StartTime.Value);

        if (localDateTime < DateTime.Now)
        {
            yield return new ValidationResult(
                "Start date and time cannot be in the past.",
                [nameof(Date), nameof(StartTime)]);
        }

        DateTime minSafeLocal = DateTimeOffset.MinValue.UtcDateTime.AddHours(14);
        DateTime maxSafeLocal = DateTimeOffset.MaxValue.UtcDateTime.AddHours(-14);

        if (localDateTime < minSafeLocal || localDateTime > maxSafeLocal)
        {
            yield return new ValidationResult(
                "Date and start time are out of supported range.",
                [nameof(Date), nameof(StartTime)]);
        }
    }
}

