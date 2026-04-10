using CoreFitness.Application.MyAccount.Outputs;
using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.MyAccount;

public interface IMyAccountBookingService
{
    Task<MyBookingsOverviewOutput?> GetOverviewAsync(string userId, CancellationToken ct = default);
    Task<IReadOnlyList<MyUpcomingSessionOutput>> GetUpcomingSessionsAsync(string userId, CancellationToken ct = default);
    Task<Result> BookSessionAsync(string userId, Guid workoutSessionId, CancellationToken ct = default);
    Task<Result> CancelSessionAsync(string userId, Guid workoutSessionId, CancellationToken ct = default);
}