namespace CoreFitness.Domain.Abstractions.Repositories;

public interface IRepositoryBase<TModel, TId>
{
    Task<TModel> AddAsync(TModel model, CancellationToken ct = default);
    Task<TModel> UpdateAsync(TId id, TModel model, CancellationToken ct = default);
    Task<bool> RemoveAsync(TId id, CancellationToken ct = default);

    Task<TModel?> GetByIdAsync(TId id, CancellationToken ct = default);
    Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken ct = default);
}
