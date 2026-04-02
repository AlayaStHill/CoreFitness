using corefitness.domain.shared.validators;

namespace CoreFitness.Domain.Aggregates.MembershipTypes.MembershipBenefits;

public sealed class MembershipBenefit
{
    public MembershipBenefitId Id { get; private set; } = default!;
    public MembershipTypeId MembershipTypeId { get; private set; } = default!;
    public string Text { get; private set; } = null!;

    private MembershipBenefit(MembershipBenefitId id, MembershipTypeId membershipTypeId, string text)
    {
        Id = id;
        MembershipTypeId = membershipTypeId;
        Text = text;
    }

    private MembershipBenefit() { }

    internal static MembershipBenefit Create(MembershipTypeId membershipTypeId, string text)
    {
        DomainValidator.RequiredGuid(membershipTypeId.Value, MembershipBenefitErrors.MembershipTypeIdRequired);
        DomainValidator.RequiredString(text, MembershipBenefitErrors.TextRequired);

        return new MembershipBenefit(MembershipBenefitId.Create(), membershipTypeId, text);
    }

    internal void UpdateText(string text)
    {
        DomainValidator.RequiredString(text, MembershipBenefitErrors.TextRequired);
        Text = text;
    }
}