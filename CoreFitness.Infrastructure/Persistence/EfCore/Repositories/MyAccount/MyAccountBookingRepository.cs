using CoreFitness.Application.MyAccount;
using CoreFitness.Application.MyAccount.Outputs;
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
}