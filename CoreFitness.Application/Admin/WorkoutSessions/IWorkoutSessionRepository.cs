using CoreFitness.Domain.Abstractions.Repositories;
using CoreFitness.Domain.Aggregates.WorkoutSessions;

namespace CoreFitness.Application.Admin.WorkoutSessions;

public interface IWorkoutSessionRepository : IRepositoryBase<WorkoutSession, WorkoutSessionId>
{
}
