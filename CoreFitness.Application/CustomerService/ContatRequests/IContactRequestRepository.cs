using CoreFitness.Domain.Abstractions.Repositories;
using CoreFitness.Domain.Aggregates.CustomerService;

namespace CoreFitness.Application.CustomerService.ContatRequests;

public interface IContactRequestRepository :IRepositoryBase<ContactRequest, string>
{
}
