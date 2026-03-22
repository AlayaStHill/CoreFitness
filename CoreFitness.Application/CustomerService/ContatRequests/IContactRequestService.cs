using CoreFitness.Application.CustomerService.ContatRequests.Inputs;
using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.CustomerService.ContatRequests;

public interface IContactRequestService
{
    Task<Result> CreateContactRequestAsync(ContactRequestInput input, CancellationToken ct = default);
}
