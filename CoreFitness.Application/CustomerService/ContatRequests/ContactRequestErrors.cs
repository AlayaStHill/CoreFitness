using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.CustomerService.ContatRequests;

public static class ContactRequestErrors
{
    public static readonly ResultError InputMustBeProvided =
        new(ErrorTypes.BadRequest, "Contact request input must be provided.");

    public static readonly ResultError IdMustBeProvided =
        new(ErrorTypes.BadRequest, "Id must be provided");

    public static readonly ResultError ContactRequestIdMustBeProvided =
        new(ErrorTypes.BadRequest, "Contact request id must be provided.");

    public static ResultError NotFoundById(string id) =>
        new(ErrorTypes.NotFound, $"Contact request with ID {id} not found.");
}
