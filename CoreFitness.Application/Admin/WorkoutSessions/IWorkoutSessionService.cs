using CoreFitness.Application.Admin.WorkoutSessions.Outputs;
using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.Admin.WorkoutSessions;

public interface IWorkoutSessionService
{
    Task<Result<IReadOnlyList<WorkoutSessionOutput>>> GetAllWorkoutSessionsAsync(CancellationToken ct = default);
    Task<Result> DeleteWorkoutSessionAsync(Guid id, CancellationToken ct = default);
}
