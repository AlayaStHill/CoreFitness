namespace CoreFitness.Presentation.WebApp.Models.Admin;

public sealed class WorkoutSessionItemViewModel
{
    public Guid Id { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTimeOffset StartsAt { get; set; }
    public TimeSpan Duration { get; set; }
    public int Capacity { get; set; }
}
