using corefitness.domain.shared.validators;
using CoreFitness.Domain.Aggregates.Memberships.MembershipTypes;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Domain.Aggregates.Memberships;

public sealed class Membership
{
    public MembershipId Id { get; private set; } = default!;
    public string UserId { get; private set; } = null!;
    public MembershipTypeId MembershipTypeId { get; private set; } = default!;
    public DateOnly StartDate { get; private set; }
    private Membership(MembershipId id, string userId, MembershipTypeId membershipTypeId, DateOnly startDate)
    {
        Id = id;
        UserId = userId;
        MembershipTypeId = membershipTypeId;
        StartDate = startDate;
    }
    private Membership() { }
    public static Membership Create(string userId, MembershipTypeId membershipTypeId, DateOnly startDate)
    {
        DomainValidator.RequiredString(userId, MembershipErrors.UserIdRequired);
        DomainValidator.RequiredGuid(membershipTypeId.Value, MembershipErrors.MembershipTypeIdRequired);

        if (startDate < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ValidationDomainException(MembershipErrors.StartDateCannotBeInThePast);

        return new(MembershipId.Create(), userId, membershipTypeId, startDate);
    }
}
