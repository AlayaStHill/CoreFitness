using CoreFitness.Application.CustomerService.ContatRequests;
using CoreFitness.Domain.Abstractions.Loggings;
using CoreFitness.Domain.Aggregates.CustomerService;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Repositories.CustomerService;

public sealed class ContactRequestRepository(PersistenceContext context) : RepositoryBase<ContactRequest, string, PersistenceContext>(context), IContactRequestRepository
{
    protected readonly PersistenceContext _context = context;

    public Task<IReadOnlyList<ContactRequest>> GetAllAsync(CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ContactRequest?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveAsync(string id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ContactRequest> UpdateAsync(string id, ContactRequest model, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
