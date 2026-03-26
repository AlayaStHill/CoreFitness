namespace CoreFitness.Domain.Aggregates.Memberships;

public class MembershipErrors
{
    public const string UserIdRequired = "UserId is required.";
    public const string StartDateCannotBeInThePast = "Start date must be in the future.";
    public const string MembershipTypeIdRequired = "MembershipTypeId is required";
}
