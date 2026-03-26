using corefitness.domain.shared.validators;

namespace CoreFitness.Domain.Aggregates.WorkoutSessions.Bookings;

public sealed class Booking
{
    public WorkoutSessionId WorkoutSessionId { get; private set; } = default!;
    public string UserId { get; private set; } = null!;
    private Booking(WorkoutSessionId workoutSessionId, string userId)
    {
        WorkoutSessionId = workoutSessionId;
        UserId = userId;
    }
    private Booking() { }
    internal static Booking Create(WorkoutSessionId workoutSessionId, string userId)
    {
        DomainValidator.RequiredGuid(workoutSessionId.Value, BookingErrors.WorkoutSessionIdRequired);
        DomainValidator.RequiredString(userId, BookingErrors.UserIdRequired);

        return new(workoutSessionId, userId);
    }
}
