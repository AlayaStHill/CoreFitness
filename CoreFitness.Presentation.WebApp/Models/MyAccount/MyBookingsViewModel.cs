namespace CoreFitness.Presentation.WebApp.Models.MyAccount;

public sealed class MyBookingsViewModel
{
    public bool HasActiveMembership { get; init; }
    public IReadOnlyList<MyBookingItemViewModel> UpcomingBookings { get; init; } = [];
    public IReadOnlyList<MyBookingItemViewModel> PreviousBookings { get; init; } = [];
}

public sealed class MyBookingItemViewModel
{
    public Guid WorkoutSessionId { get; init; }
    public string WorkoutTitle { get; init; } = string.Empty;
    public string WorkoutCategory { get; init; } = string.Empty;
    public DateTimeOffset StartsAt { get; init; }
    public int DurationMinutes { get; init; }
}