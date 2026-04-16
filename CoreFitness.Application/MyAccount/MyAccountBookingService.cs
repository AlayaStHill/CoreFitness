using CoreFitness.Application.MyAccount.Outputs;
using CoreFitness.Application.Shared;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.WorkoutSessions.Services;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Application.MyAccount;

public sealed class MyAccountBookingService(
    IMyAccountBookingRepository bookingRepository,
    WorkoutBookingDomainService workoutBookingDomainService,
    IUnitOfWork unitOfWork) : IMyAccountBookingService
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

    public async Task<IReadOnlyList<MyUpcomingSessionOutput>> GetUpcomingSessionsAsync(string userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return [];

        return await bookingRepository.GetUpcomingSessionsByUserIdAsync(userId, ct);
    }

    public async Task<Result> BookSessionAsync(string userId, Guid workoutSessionId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Result.Fail(MyAccountErrors.UserIdRequired);

        if (workoutSessionId == Guid.Empty)
            return Result.Fail(MyAccountErrors.WorkoutSessionIdRequired);

        var member = await bookingRepository.GetMemberByUserIdForBookingAsync(userId, ct);
        if (member is null)
            return Result.Fail(MyAccountErrors.MemberNotFound);

        var workoutSession = await bookingRepository.GetWorkoutSessionByIdForBookingAsync(workoutSessionId, ct);
        if (workoutSession is null)
            return Result.Fail(MyAccountErrors.WorkoutSessionNotFound);

        try
        {
            workoutBookingDomainService.Book(member, workoutSession);
        }
        catch (ValidationDomainException ex)
        {
            return Result.Fail(MyAccountErrors.Validation(ex.Message));
        }

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> CancelSessionAsync(string userId, Guid workoutSessionId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return Result.Fail(MyAccountErrors.UserIdRequired);

        if (workoutSessionId == Guid.Empty)
            return Result.Fail(MyAccountErrors.WorkoutSessionIdRequired);

        var member = await bookingRepository.GetMemberByUserIdForBookingAsync(userId, ct);
        if (member is null)
            return Result.Fail(MyAccountErrors.MemberNotFound);

        var workoutSession = await bookingRepository.GetWorkoutSessionByIdForBookingAsync(workoutSessionId, ct);
        if (workoutSession is null)
            return Result.Fail(MyAccountErrors.WorkoutSessionNotFound);

        try
        {
            workoutSession.CancelBooking(member.Id);
        }
        catch (ValidationDomainException ex)
        {
            return Result.Fail(MyAccountErrors.Validation(ex.Message));
        }

        await unitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}