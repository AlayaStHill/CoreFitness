using CoreFitness.Domain.Aggregates.WorkoutTypes;

namespace CoreFitness.Application.Admin.WorkoutSessions.Inputs;

public sealed class CreateWorkoutSessionInput
{
    public required WorkoutTypeId WorkoutTypeId { get; init; }
    public DateTimeOffset StartsAt { get; init; }
    public TimeSpan Duration { get; init; }
    public int Capacity { get; init; }
}
