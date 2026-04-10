namespace CoreFitness.Presentation.WebApp.Models.MyAccount;

public sealed class NewBookingViewModel
{
    public bool HasActiveMembership { get; init; }
    public IReadOnlyList<NewBookingSessionItemViewModel> Sessions { get; init; } = [];
}

public sealed class NewBookingSessionItemViewModel
{
    public Guid WorkoutSessionId { get; init; }
    public string WorkoutTitle { get; init; } = string.Empty;
    public string WorkoutCategory { get; init; } = string.Empty;
    public DateTimeOffset StartsAt { get; init; }
    public int DurationMinutes { get; init; }
    public int BookedCount { get; init; }
    public int Capacity { get; init; }
    public bool IsAlreadyBooked { get; init; }
    public int AvailableSpots => Math.Max(Capacity - BookedCount, 0);
    public bool IsFullyBooked => BookedCount >= Capacity;
    public bool CanBook => !IsAlreadyBooked && !IsFullyBooked;
}