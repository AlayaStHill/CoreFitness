using CoreFitness.Application.Admin.WorkoutSessions.Outputs;
using CoreFitness.Application.Shared;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.WorkoutSessions;

namespace CoreFitness.Application.Admin.WorkoutSessions;

public sealed class WorkoutSessionService(IWorkoutSessionRepository workoutSessionRepository, IUnitOfWork unitOfWork) : IWorkoutSessionService
{
    public async Task<Result> DeleteWorkoutSessionAsync(WorkoutSessionId id, CancellationToken ct = default)
    {
        if (id == default)
            return Result.Fail(ErrorTypes.BadRequest, "Id must be provided");
        
        bool isRemoved = await workoutSessionRepository.RemoveAsync(id, ct);
        if (!isRemoved)
            return Result.Fail(ErrorTypes.NotFound, $"Workout session with ID {id} not found");

        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<WorkoutSessionOutput>>> GetAllWorkoutSessionsAsync(CancellationToken ct = default)
    {
        IReadOnlyList<WorkoutSession> workoutSessions = await workoutSessionRepository.GetAllAsync(ct);

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