using CoreFitness.Application.Admin.WorkoutTypes;
using CoreFitness.Domain.Aggregates.WorkoutTypes;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Repositories.Admin.WorkoutTypes;

public sealed class WorkoutTypeRepository(PersistenceContext context) : RepositoryBase<WorkoutType, WorkoutTypeId, PersistenceContext>(context), IWorkoutTypeRepository
{
    public override async Task<IReadOnlyList<WorkoutType>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<WorkoutType>()
            .AsNoTracking()
            .Include(wt => wt.WorkoutCategory)
            .ToListAsync(ct);
    }
}
