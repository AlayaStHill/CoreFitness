using CoreFitness.Application.CustomerService.ContatRequests.Inputs;
using CoreFitness.Application.Shared;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.CustomerService;

namespace CoreFitness.Application.CustomerService.ContatRequests;

public sealed class ContactRequestService(IContactRequestRepository _repository, IUnitOfWork _iUnitOfWork) : IContactRequestService
{
    public async Task<Result> CreateContactRequestAsync(ContactRequestInput input, CancellationToken ct = default)
    {
       if (input is null)
            return Result.Fail(ErrorTypes.BadRequest, "Contact request input must be provided.");

        ContactRequest request = ContactRequest.Create(input.FirstName, input.LastName, input.Email, input.PhoneNumber, input.Message);

        await _repository.AddAsync(request, ct);
        await _iUnitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }
}
