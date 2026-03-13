using CoreFitness.Domain.Abstractions.Loggings;
using CoreFitness.Domain.Abstractions.Repositories;
using CoreFitness.Domain.Exceptions.Custom;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Repositories;

public abstract class RepositoryBase<TModel, TId, TEntity, TDbContext>(TDbContext context, IDomainLogger logger)
    : IRepositoryBase<TModel, TId>
    where TEntity : class
    where TDbContext : DbContext
{
    protected readonly TDbContext _context = context;
    protected readonly IDomainLogger _logger = logger;
    protected DbSet<TEntity> Set => _context.Set<TEntity>();

    // ApplyPropertyUpdates??? och Inte ha SaveChanges i repot --> application
    protected abstract void ApplyUpdates(TModel model, TEntity entity);
    protected abstract TModel ToModel(TEntity entity);
    protected abstract TEntity ToEntity(TModel model);

    public virtual async Task<TModel> AddAsync(TModel model, CancellationToken ct = default)
    {
        if (model is null)
            // Ger felmeddelande med standardtext och parameternamn, ex. Value cannot be null. (Parameter 'model')
            throw new ArgumentNullException(nameof(model));

        TEntity entity = ToEntity(model);

        await Set.AddAsync(entity, ct);

        return ToModel(entity);
    }

    public virtual async Task<TModel> UpdateAsync(TId id, TModel model, CancellationToken ct = default)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        TEntity? entity = await Set.FindAsync([id], ct)
            ?? throw new NotFoundDomainException($"Entity with id {id} was not found");

        ApplyUpdates(model, entity);

        return ToModel(entity);
    }

    public virtual async Task<bool> RemoveAsync(TId id, CancellationToken ct = default)
    {
        TEntity? entity = await Set.FindAsync([id], ct);
        if (entity is null)
            return false;

        Set.Remove(entity);
        return true;
    }

    public virtual async Task<TModel?> GetByIdAsync(TId id, CancellationToken ct = default)
    {
        // FindAsync tar en array av nycklar (ev. composite keys)
        TEntity? entity = await Set.FindAsync([id], ct);
        return entity is null ? default : ToModel(entity);
    }

    public virtual async Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken ct = default)
    {
        List<TEntity>? entities = await Set.AsNoTracking().ToListAsync(ct);
        // .. spread operator, [...] collection expression - ta varje element i entities och skapa en ny lista av TModel
        return [.. entities.Select(ToModel)];
    }
}
