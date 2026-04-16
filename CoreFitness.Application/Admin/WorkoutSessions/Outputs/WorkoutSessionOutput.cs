namespace CoreFitness.Application.Admin.WorkoutSessions.Outputs;

public sealed class WorkoutSessionOutput
{
    public Guid Id { get; init; }
    public string Category { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public DateTimeOffset StartsAt { get; init; }
    public TimeSpan Duration { get; init; }
    public int Capacity { get; init; }
}
