namespace CoreFitness.Application.MyAccount.Outputs;

public sealed record MyBookingsOverviewOutput(
    IReadOnlyList<MyBookingItemOutput> UpcomingBookings,
    IReadOnlyList<MyBookingItemOutput> PreviousBookings
);