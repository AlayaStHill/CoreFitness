namespace CoreFitness.Domain.Aggregates.Members;

public static class MemberErrors
{
    public const string UserIdRequired = "User id is required.";
    public const string MemberAlreadyHasActiveMembership = "Member already has an active membership.";
    public const string MemberHasNoMembership = "Member has no membership.";
    public const string MemberHasNoPausedMembership = "Member has no paused membership.";

}
