namespace CoreFitness.Domain.Aggregates.WorkoutSessions;

public static class WorkoutSessionErrors
{
    public const string InvalidStartsAt = "Workout session start time must be in the future.";
    public const string InvalidDuration = "Workout session duration must be greater than zero.";
    public const string InvalidCapacity = "Workout session capacity must be greater than zero.";
    public const string WorkoutTypeIdRequired = "Workout type id is required.";
    public const string MemberIdRequired = "Member id is required.";
    public const string BookingNotAllowedForStartedSession = "Booking is not allowed for a session that has started.";
    public const string MemberAlreadyBooked = "Member has already booked this workout session.";
    public const string SessionIsFull = "Workout session is full.";
    public const string BookingNotFound = "Booking was not found for this member.";
}
