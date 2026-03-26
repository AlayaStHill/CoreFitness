using corefitness.domain.shared.validators;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Domain.Aggregates.Members.MembershipTypes;

public sealed class MembershipType
{
    public MembershipTypeId Id { get; private set; } = default!;
    public string Name { get; private set; } = null!;
    public decimal PricePerMonth { get; private set; }
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

        return new(MembershipTypeId.Create(), name, price);
    }
}
