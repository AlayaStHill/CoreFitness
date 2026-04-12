using CoreFitness.Application.Admin.WorkoutSessions;
using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Repositories.Admin;

public sealed class WorkoutSessionRepository(PersistenceContext context) : RepositoryBase<WorkoutSession, string, PersistenceContext>(context), IWorkoutSessionsRepository
{
    public async override Task<IReadOnlyList<WorkoutSession>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.WorkoutSessions
            .AsNoTracking()
            .Include(ws => ws.WorkoutType)
            .ThenInclude(wt => wt.WorkoutCategory)
            .ToListAsync(ct);
    }
}
