namespace CoreFitness.Application.MyAccount.Outputs;

public sealed record MyUpcomingSessionOutput(
    Guid WorkoutSessionId,
    string WorkoutTitle,
    string WorkoutCategory,
    DateTimeOffset StartsAt,
    int DurationMinutes,
    int BookedCount,
    int Capacity,
    bool IsAlreadyBooked
);