namespace CoreFitness.Application.MyAccount.Outputs;

public sealed record MyBookingItemOutput(
    Guid WorkoutSessionId,
    string WorkoutTitle,
    string WorkoutCategory,
    DateTimeOffset StartsAt,
    int DurationMinutes
);