using CoreFitness.Application.Admin.WorkoutSessions.Outputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.WorkoutSessions;

namespace CoreFitness.Application.Admin.WorkoutSessions;

public sealed class WorkoutSessionService(IWorkoutSessionsRepository workoutSessionsRepository) : IWorkoutSessionService
{
    public async Task<Result<IReadOnlyList<WorkoutSessionOutput>>> GetAllWorkoutSessionsAsync(CancellationToken ct = default)
    {
        IReadOnlyList<WorkoutSession> workoutSessions = await workoutSessionsRepository.GetAllAsync(ct);

        IReadOnlyList<WorkoutSessionOutput> workoutSessionOutputs = workoutSessions
            .Select(ws => new WorkoutSessionOutput
            {
                Id = ws.Id.Value,
                Category = ws.WorkoutType.WorkoutCategory.Title,
                Type = ws.WorkoutType.Title,
                StartsAt = ws.StartsAt,
                Duration = ws.Duration,
                Capacity = ws.Capacity
            })
            .ToList();

        return Result<IReadOnlyList<WorkoutSessionOutput>>.Success(workoutSessionOutputs);
    }
}