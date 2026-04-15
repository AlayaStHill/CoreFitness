using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.MembershipTypes;

public static class MembershipTypeErrors
{
    public static readonly ResultError SomethingWentWrong =
        new(ErrorTypes.Error, "Something went wrong.");
}
