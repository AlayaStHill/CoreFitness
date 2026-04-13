using CoreFitness.Application.Admin.WorkoutSessions.Outputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.WorkoutSessions;

namespace CoreFitness.Application.Admin.WorkoutSessions;

public interface IWorkoutSessionService
{
    Task<Result<IReadOnlyList<WorkoutSessionOutput>>> GetAllWorkoutSessionsAsync(CancellationToken ct = default);
    Task<Result> DeleteWorkoutSessionAsync(WorkoutSessionId id, CancellationToken ct = default);
}
