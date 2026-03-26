namespace CoreFitness.Domain.Aggregates.Memberships;

public static class MembershipErrors
{
    public const string UserIdRequired = "User id is required.";
    public const string MembershipTypeIdRequired = "Membership type id is required.";
    public const string StartDateCannotBeInThePast = "Start date cannot be in the past.";

    public const string MembershipAlreadyCancelled = "Membership is already cancelled.";
    public const string OnlyActiveMembershipCanBePaused = "Only active memberships can be paused.";
    public const string MembershipAlreadyActive = "Membership is already active.";
}
