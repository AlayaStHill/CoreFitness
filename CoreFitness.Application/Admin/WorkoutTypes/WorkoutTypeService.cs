using CoreFitness.Application.Admin.WorkoutTypes.Outputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.WorkoutTypes;

namespace CoreFitness.Application.Admin.WorkoutTypes;

public sealed class WorkoutTypeService(IWorkoutTypeRepository workoutTypeRepository) : IWorkoutTypeService
{
    public async Task<Result<IReadOnlyList<WorkoutTypeOutput>>> GetWorkoutTypesAsync(CancellationToken ct = default)
    {
        IReadOnlyList<WorkoutType> workoutTypes = await workoutTypeRepository.GetAllAsync(ct);

        var workoutTypeOutputs = workoutTypes
            .Select(wt => new WorkoutTypeOutput
            {
                Id = wt.Id.Value,
                Title = wt.Title,
                WorkoutCategory = wt.WorkoutCategory.Title
            })
            .ToList();

        return Result<IReadOnlyList<WorkoutTypeOutput>>.Success(workoutTypeOutputs);
    }
}

