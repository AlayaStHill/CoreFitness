using corefitness.domain.shared.validators;
using CoreFitness.Domain.Aggregates.Workouts.WorkoutTypes;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Domain.Aggregates.Workouts.WorkoutSessions;

public sealed class WorkoutSession
{
    public WorkoutSessionId Id { get; private set; } = default!;
    public WorkoutTypeId WorkoutTypeId { get; private set; } = default!;
    public DateTimeOffset StartsAt { get; private set; }
    public TimeSpan Duration { get; private set; }
    public int Capacity { get; private set; }
    private WorkoutSession(WorkoutSessionId id, WorkoutTypeId workoutTypeId, DateTimeOffset startsAt, TimeSpan duration, int capacity)
    {
        Id = id;
        StartsAt = startsAt;
        Duration = duration;
        Capacity = capacity;
        WorkoutTypeId = workoutTypeId;
    }
    private WorkoutSession() { }
    public static WorkoutSession Create(DateTimeOffset startsAt, WorkoutTypeId workoutTypeId, TimeSpan duration, int capacity)
    {
        if (startsAt < DateTime.UtcNow)
            throw new ValidationDomainException(WorkoutSessionErrors.InvalidStartsAt);
        if (duration <= TimeSpan.Zero)
            throw new ValidationDomainException(WorkoutSessionErrors.InvalidDuration);

        DomainValidator.RequiredGuid(workoutTypeId.Value, WorkoutSessionErrors.WorkoutTypeIdRequired);

        return new(WorkoutSessionId.Create(), workoutTypeId, startsAt, duration, capacity);
    }
}
