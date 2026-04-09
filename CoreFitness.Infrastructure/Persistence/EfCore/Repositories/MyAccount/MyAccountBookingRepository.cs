using CoreFitness.Application.MyAccount;
using CoreFitness.Application.MyAccount.Outputs;
using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Repositories.MyAccount;

public sealed class MyAccountBookingRepository(PersistenceContext context) : IMyAccountBookingRepository
{
    public async Task<IReadOnlyList<MyBookingItemOutput>> GetByUserIdAsync(string userId, CancellationToken ct = default)
    {
        return await context.Members
            .AsNoTracking()
            .Where(member => member.UserId == userId)
            .Join(
                context.Bookings.AsNoTracking(),
                member => member.Id,
                booking => booking.MemberId,
                (_, booking) => booking)
            .Join(
                context.WorkoutSessions.AsNoTracking(),
                booking => booking.WorkoutSessionId,
                workoutSession => workoutSession.Id,
                (booking, workoutSession) => new { booking, workoutSession })
            .Join(
                context.WorkoutTypes.AsNoTracking(),
                item => item.workoutSession.WorkoutTypeId,
                workoutType => workoutType.Id,
                (item, workoutType) => new { item.workoutSession, workoutType })
            .Join(
                context.WorkoutCategories.AsNoTracking(),
                item => item.workoutType.WorkoutCategoryId,
                workoutCategory => workoutCategory.Id,
                (item, workoutCategory) => new MyBookingItemOutput(
                    item.workoutSession.Id.Value,
                    item.workoutType.Title,
                    workoutCategory.Title,
                    item.workoutSession.StartsAt,
                    (int)item.workoutSession.Duration.TotalMinutes))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<MyUpcomingSessionOutput>> GetUpcomingSessionsByUserIdAsync(string userId, CancellationToken ct = default)
    {
        var memberId = await context.Members
            .AsNoTracking()
            .Where(member => member.UserId == userId)
            .Select(member => member.Id)
            .FirstOrDefaultAsync(ct);

        if (memberId is null)
            return [];

        var now = DateTimeOffset.UtcNow;

        return await context.WorkoutSessions
            .AsNoTracking()
            .Where(workoutSession => workoutSession.StartsAt > now)
            .OrderBy(workoutSession => workoutSession.StartsAt)
            .Join(
                context.WorkoutTypes.AsNoTracking(),
                workoutSession => workoutSession.WorkoutTypeId,
                workoutType => workoutType.Id,
                (workoutSession, workoutType) => new { workoutSession, workoutType })
            .Join(
                context.WorkoutCategories.AsNoTracking(),
                item => item.workoutType.WorkoutCategoryId,
                workoutCategory => workoutCategory.Id,
                (item, workoutCategory) => new MyUpcomingSessionOutput(
                    item.workoutSession.Id.Value,
                    item.workoutType.Title,
                    workoutCategory.Title,
                    item.workoutSession.StartsAt,
                    (int)item.workoutSession.Duration.TotalMinutes,
                    context.Bookings.Count(booking => booking.WorkoutSessionId == item.workoutSession.Id),
                    item.workoutSession.Capacity,
                    context.Bookings.Any(booking => booking.WorkoutSessionId == item.workoutSession.Id && booking.MemberId == memberId)))
            .ToListAsync(ct);
    }

    public async Task<Member?> GetMemberByUserIdForBookingAsync(string userId, CancellationToken ct = default)
    {
        return await context.Members
            .Include(member => member.Memberships)
            .FirstOrDefaultAsync(member => member.UserId == userId, ct);
    }

    public async Task<WorkoutSession?> GetWorkoutSessionByIdForBookingAsync(Guid workoutSessionId, CancellationToken ct = default)
    {
        var sessionId = new WorkoutSessionId(workoutSessionId);

        return await context.WorkoutSessions
            .Include(workoutSession => workoutSession.Bookings)
            .FirstOrDefaultAsync(workoutSession => workoutSession.Id == sessionId, ct);
    }
}