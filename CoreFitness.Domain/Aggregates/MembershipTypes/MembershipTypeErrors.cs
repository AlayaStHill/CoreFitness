namespace CoreFitness.Domain.Aggregates.MembershipTypes;

public static class MembershipTypeErrors
{
    public const string NameRequired = "Membership type name is required.";
    public const string DescriptionRequired = "Description is required.";
    public const string PriceMustBePositive = "Price must be zero or greater.";
    public const string ClassesPerMonthMustBePositive = "Classes per month must be zero or greater.";
}