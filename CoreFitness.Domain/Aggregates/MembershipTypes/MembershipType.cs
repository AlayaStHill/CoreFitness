using corefitness.domain.shared.validators;
using CoreFitness.Domain.Aggregates.MembershipTypes.MembershipBenefits;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Domain.Aggregates.MembershipTypes;

public sealed class MembershipType
{
    private readonly List<MembershipBenefit> _benefits = [];

    public MembershipTypeId Id { get; private set; } = default!;
    public string Name { get; private set; } = null!;
    public decimal PricePerMonth { get; private set; }

    public IReadOnlyCollection<MembershipBenefit> Benefits => _benefits.AsReadOnly();

    private MembershipType(MembershipTypeId id, string name, decimal price)
    {
        Id = id;
        Name = name;
        PricePerMonth = price;
    }

    private MembershipType() { }

    public static MembershipType Create(string name, decimal price)
    {
        DomainValidator.RequiredString(name, MembershipTypeErrors.NameRequired);

        if (price < 0)
        {
            throw new ValidationDomainException(MembershipTypeErrors.PriceMustBePositive);
        }

        return new MembershipType(MembershipTypeId.Create(), name, price);
    }

    public void AddBenefit(string text)
    {
        DomainValidator.RequiredString(text, MembershipBenefitErrors.TextRequired);

        if (_benefits.Any(x => x.Text == text))
        {
            throw new ValidationDomainException(MembershipBenefitErrors.DuplicateBenefitText);
        }

        _benefits.Add(MembershipBenefit.Create(Id, text));
    }

    public void UpdateBenefit(MembershipBenefitId benefitId, string text)
    {
        var benefit = _benefits.FirstOrDefault(x => x.Id == benefitId);

        if (benefit is null)
        {
            throw new ValidationDomainException(MembershipBenefitErrors.BenefitNotFound);
        }

        if (_benefits.Any(x => x.Id != benefitId && x.Text == text))
        {
            throw new ValidationDomainException(MembershipBenefitErrors.DuplicateBenefitText);
        }

        benefit.UpdateText(text);
    }

    public void RemoveBenefit(MembershipBenefitId benefitId)
    {
        var benefit = _benefits.FirstOrDefault(x => x.Id == benefitId);

        if (benefit is null)
        {
            throw new ValidationDomainException(MembershipBenefitErrors.BenefitNotFound);
        }

        _benefits.Remove(benefit);
    }
}