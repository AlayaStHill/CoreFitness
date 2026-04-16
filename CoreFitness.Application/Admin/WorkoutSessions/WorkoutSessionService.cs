using CoreFitness.Application.Admin.WorkoutSessions.Inputs;
using CoreFitness.Application.Admin.WorkoutSessions.Outputs;
using CoreFitness.Application.Shared;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.WorkoutSessions;

namespace CoreFitness.Application.Admin.WorkoutSessions;

public sealed class WorkoutSessionService(IWorkoutSessionRepository workoutSessionRepository, IUnitOfWork unitOfWork) : IWorkoutSessionService
{
    public async Task<Result> CreateWorkoutSessionAsync(CreateWorkoutSessionInput input, CancellationToken ct = default)
    {
        if (input is null)
            return Result.Fail(ErrorTypes.BadRequest, WorkoutSessionServiceErrors.InputIsRequired);

        WorkoutSession session = WorkoutSession.Create(input.StartsAt, input.WorkoutTypeId,  input.Duration, input.Capacity);
        
        await workoutSessionRepository.AddAsync(session, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteWorkoutSessionAsync(WorkoutSessionId id, CancellationToken ct = default)
    {
        if (id == default)
            return Result.Fail(ErrorTypes.BadRequest, WorkoutSessionServiceErrors.IdIsRequired);
        
        bool isRemoved = await workoutSessionRepository.RemoveAsync(id, ct);
        if (!isRemoved)
            return Result.Fail(WorkoutSessionServiceErrors.WorkoutSessionNotFound(id));
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