using CoreFitness.Domain.Abstractions.Repositories;
using CoreFitness.Domain.Aggregates.WorkoutSessions;

namespace CoreFitness.Application.Admin.WorkoutSessions;

public interface IWorkoutSessionsRepository : IRepositoryBase<WorkoutSession, Guid>
{
}
