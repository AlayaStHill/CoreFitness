namespace CoreFitness.Domain.Aggregates.Members.Memberships;

public static class MembershipErrors
{
    public const string MemberIdRequired = "Member id is required.";
    public const string MembershipTypeIdRequired = "Membership type id is required.";

    public const string MembershipAlreadyCancelled = "Membership is already cancelled.";
    public const string MembershipAlreadyActive = "Membership is already active.";

    public const string OnlyActiveMembershipCanBePaused = "Only active memberships can be paused.";
    public const string CancelledMembershipCannotBeActivated = "Cancelled membership cannot be activated.";

    public const string CancelledMembershipCannotBeChanged = "Cancelled membership cannot be changed.";
    public const string MembershipTypeIsAlreadySelected = "Membership type is already selected.";
}
