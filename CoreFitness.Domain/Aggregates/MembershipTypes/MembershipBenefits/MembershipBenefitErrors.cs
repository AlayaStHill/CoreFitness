namespace CoreFitness.Domain.Aggregates.MembershipTypes.MembershipBenefits;

public static class MembershipBenefitErrors
{
    public const string MembershipTypeIdRequired = "Membership type id is required.";
    public const string TextRequired = "Benefit text is required.";
    public const string BenefitNotFound = "Benefit was not found.";
    public const string DuplicateBenefitText = "This benefit already exists for the membership type.";
}
