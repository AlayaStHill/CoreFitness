using CoreFitness.Application.CustomerService.ContatRequests.Inputs;
using CoreFitness.Application.Shared;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Domain.Aggregates.CustomerService;

namespace CoreFitness.Application.CustomerService.ContatRequests;

public sealed class ContactRequestService(IContactRequestRepository repository, IUnitOfWork iUnitOfWork) : IContactRequestService
{
    public async Task<Result> CreateContactRequestAsync(ContactRequestInput input, CancellationToken ct = default)
    {
       if (input is null)
            return Result.Fail(ErrorTypes.BadRequest, "Contact request input must be provided.");

        ContactRequest request = ContactRequest.Create(input.FirstName, input.LastName, input.Email, input.PhoneNumber, input.Message);

        await repository.AddAsync(request, ct);
        await iUnitOfWork.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> DeleteContactRequestAsync(string id, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Result.Fail(ErrorTypes.BadRequest, "Id must be provided");

        bool isRemoved = await repository.RemoveAsync(id, ct);

        if (!isRemoved)
            return Result.Fail(ErrorTypes.NotFound, $"Contact request with ID {id} not found.");

        await iUnitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result<IReadOnlyList<ContactRequest>>> GetAllContactRequestsAsync(CancellationToken ct = default)
    {
        IReadOnlyList<ContactRequest> contactRequests = await repository.GetAllAsync(ct);

        return Result<IReadOnlyList<ContactRequest>>.Success(contactRequests);
    }

    public async Task<Result<ContactRequest>> GetContactRequestAsync(string id, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Result<ContactRequest>.Fail(ErrorTypes.BadRequest, "Contact request id must be provided.");

        ContactRequest? contactRequest = await repository.GetByIdAsync(id, ct);

        return contactRequest is null
            ? Result<ContactRequest>.Fail(ErrorTypes.NotFound, $"Contact request with ID {id} not found.")
            : Result<ContactRequest>.Success(contactRequest);
    }

    public async Task<Result> MarkAsReadAsync(string id, CancellationToken ct = default)
    {
        Result<ContactRequest> result = await GetContactRequestOrFailAsync(id, ct);

        if (result.IsFailure)
            return Result.Fail(result.Error!);

        ContactRequest contactRequest = result.Value;

        // returnerar true om den var markerad som läst
        bool wasRead = contactRequest.MarkedAsRead;
        // markerar till läst
        contactRequest.MarkAsRead();

        // spara bara om den inte var läs innnan
        if (!wasRead)
            await iUnitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> MarkAsUnreadAsync(string id, CancellationToken ct = default)
    {
        Result<ContactRequest> result = await GetContactRequestOrFailAsync(id, ct);

        if (result.IsFailure)
            return Result.Fail(result.Error!);

        ContactRequest contactRequest = result.Value;

        // returnerar true om den var markerad som läst
        bool wasRead = contactRequest.MarkedAsRead;
        // markerar till oläst
        contactRequest.MarkAsUnread();

        if (wasRead)
            await iUnitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }

    private async Task<Result<ContactRequest>> GetContactRequestOrFailAsync(string id, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Result<ContactRequest>.Fail(ErrorTypes.BadRequest, "Id must be provided");

        ContactRequest? contactRequest = await repository.GetByIdAsync(id, ct);

        return contactRequest is null
            ? Result<ContactRequest>.Fail(ErrorTypes.NotFound, $"Contact request with ID {id} not found.")
            : Result<ContactRequest>.Success(contactRequest);
    }
}
