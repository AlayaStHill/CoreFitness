using CoreFitness.Application.MyAccount.Outputs;
using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Domain.Aggregates.WorkoutSessions;

namespace CoreFitness.Application.MyAccount;

public interface IMyAccountBookingRepository
{
    Task<IReadOnlyList<MyBookingItemOutput>> GetByUserIdAsync(string userId, CancellationToken ct = default);
    Task<IReadOnlyList<MyUpcomingSessionOutput>> GetUpcomingSessionsByUserIdAsync(string userId, CancellationToken ct = default);
    Task<Member?> GetMemberByUserIdForBookingAsync(string userId, CancellationToken ct = default);
    Task<WorkoutSession?> GetWorkoutSessionByIdForBookingAsync(Guid workoutSessionId, CancellationToken ct = default);
}