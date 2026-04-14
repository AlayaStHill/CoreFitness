using CoreFitness.Application.Admin.WorkoutTypes.Outputs;
using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.Admin.WorkoutTypes;

public interface IWorkoutTypeService
{
    Task<Result<IReadOnlyList<WorkoutTypeOutput>>> GetWorkoutTypesAsync(CancellationToken ct = default);

}
