using CoreFitness.Application.MyAccount.Outputs;

namespace CoreFitness.Application.MyAccount;

public sealed class MyAccountBookingService(IMyAccountBookingRepository bookingRepository) : IMyAccountBookingService
{
    public async Task<MyBookingsOverviewOutput?> GetOverviewAsync(string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return null;

        var bookings = await bookingRepository.GetByUserIdAsync(userId, ct);

        var now = DateTimeOffset.UtcNow;

        var upcomingBookings = bookings
            .Where(booking => booking.StartsAt >= now)
            .OrderBy(booking => booking.StartsAt)
            .ToList();

        var previousBookings = bookings
            .Where(booking => booking.StartsAt < now)
            .OrderByDescending(booking => booking.StartsAt)
            .ToList();

        return new MyBookingsOverviewOutput(upcomingBookings, previousBookings);
    }
}