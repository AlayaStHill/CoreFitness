namespace CoreFitness.Application.Shared;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct);

}
