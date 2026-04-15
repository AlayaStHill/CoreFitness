using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.MyAccount;

public static class MyAccountErrors
{
    public static readonly ResultError UserIdRequired =
        new(ErrorTypes.BadRequest, "User id must be provided.");

    public static readonly ResultError WorkoutSessionIdRequired =
        new(ErrorTypes.BadRequest, "Workout session id must be provided.");

    public static readonly ResultError MembershipTypeIdRequired =
        new(ErrorTypes.BadRequest, "Membership type id must be provided.");

    public static readonly ResultError MemberNotFound =
        new(ErrorTypes.NotFound, "Member not found.");

    public static readonly ResultError WorkoutSessionNotFound =
        new(ErrorTypes.NotFound, "Workout session not found.");

    public static readonly ResultError UserNotFound =
        new(ErrorTypes.NotFound, "User not found.");

    public static readonly ResultError SelectedMembershipPlanNotFound =
        new(ErrorTypes.NotFound, "Selected membership plan was not found.");

    public static ResultError Validation(string message) =>
        new(ErrorTypes.BadRequest, message);

    public static ResultError IdentityUpdateFailed(string message) =>
        new(ErrorTypes.BadRequest, message);
}
