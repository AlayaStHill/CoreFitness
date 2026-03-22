using CoreFitness.Application.CustomerService.ContatRequests;
using CoreFitness.Domain.Aggregates.CustomerService;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Repositories.CustomerService;

public sealed class ContactRequestRepository(PersistenceContext context) : RepositoryBase<ContactRequest, string, PersistenceContext>(context), IContactRequestRepository
{
}
