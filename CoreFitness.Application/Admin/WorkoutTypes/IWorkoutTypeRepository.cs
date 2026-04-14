using CoreFitness.Domain.Abstractions.Repositories;
using CoreFitness.Domain.Aggregates.WorkoutTypes;

namespace CoreFitness.Application.Admin.WorkoutTypes;

public interface IWorkoutTypeRepository : IRepositoryBase<WorkoutType, WorkoutTypeId>
{
}
