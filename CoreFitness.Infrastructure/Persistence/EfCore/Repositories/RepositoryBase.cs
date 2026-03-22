using CoreFitness.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Repositories;

public abstract class RepositoryBase<TModel, TId, TDbContext>(TDbContext context)
    : IRepositoryBase<TModel, TId>
    where TModel : class
    where TDbContext : DbContext
{
    protected readonly TDbContext _context = context;
    protected DbSet<TModel> Set => _context.Set<TModel>();

    public virtual async Task<TModel> AddAsync(TModel model, CancellationToken ct = default)
    {
        // Metoden anropas fel, programmeringsfel
        if (model is null)
            // Ger felmeddelande med standardtext och parameternamn, ex. Value cannot be null. (Parameter 'model')
            throw new ArgumentNullException(nameof(model));

        await Set.AddAsync(model, ct);

        return model;
    }

    public virtual async Task<TModel?> UpdateAsync(TId id, TModel model, CancellationToken ct = default)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        TModel? existing = await Set.FindAsync([id], ct);

        if (existing is null) return null;

        // Kopiera värden från model → existing (EF trackar existing)
        _context.Entry(existing).CurrentValues.SetValues(model);

        return existing;
    }

    public virtual async Task<bool> RemoveAsync(TId id, CancellationToken ct = default)
    {
        TModel? entity = await Set.FindAsync([id], ct);
        if (entity is null)
            return false;

        Set.Remove(entity);
        return true;
    }

    public virtual async Task<TModel?> GetByIdAsync(TId id, CancellationToken ct = default)
    {
        // FindAsync tar en array av nycklar (ev. composite keys)
        TModel? entity = await Set.FindAsync([id], ct);
        return entity is null ? default : entity;
    }

    public virtual async Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken ct = default)
    {
        List<TModel>? entities = await Set.AsNoTracking().ToListAsync(ct);
        // .. spread operator, [...] collection expression - ta varje element i entities och skapa en ny lista av TModel
        return entities;
    }
}
