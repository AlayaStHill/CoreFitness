using CoreFitness.Application.CustomerService.ContatRequests.Inputs;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.CustomerService;

namespace CoreFitness.Application.CustomerService.ContatRequests;

public interface IContactRequestService
{
    Task<Result> CreateContactRequestAsync(ContactRequestInput input, CancellationToken ct = default);
    Task<Result<ContactRequest>> GetContactRequestAsync(string id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ContactRequest>>> GetAllContactRequestsAsync(CancellationToken ct = default);
    Task<Result> DeleteContactRequestAsync(string id, CancellationToken ct = default);
    Task<Result> MarkAsReadAsync(string id, CancellationToken ct = default);
    Task<Result> MarkAsUnreadAsync(string id, CancellationToken ct = default);
}
