using corefitness.domain.shared.validators;
using CoreFitness.Domain.Aggregates.Members;

namespace CoreFitness.Domain.Aggregates.WorkoutSessions.Bookings;

public sealed class Booking
{
    public WorkoutSessionId WorkoutSessionId { get; private set; } = default!;
    public MemberId MemberId { get; private set; } = null!;
    private Booking(WorkoutSessionId workoutSessionId, MemberId memberId)
    {
        WorkoutSessionId = workoutSessionId;
        MemberId = memberId;
    }
    private Booking() { }
    internal static Booking Create(WorkoutSessionId workoutSessionId, MemberId memberId)
    {
        DomainValidator.RequiredGuid(workoutSessionId.Value, BookingErrors.WorkoutSessionIdRequired);
        DomainValidator.RequiredGuid(memberId.Value, BookingErrors.UserIdRequired);

        return new(workoutSessionId, memberId);
    }
}
