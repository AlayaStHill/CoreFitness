using corefitness.domain.shared.validators;
using CoreFitness.Domain.Aggregates.MembershipTypes;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Domain.Aggregates.Memberships;

public sealed class Membership
{
    public MembershipId Id { get; private set; } = default!;
    public string UserId { get; private set; } = null!;
    public MembershipTypeId MembershipTypeId { get; private set; } = default!;
    public DateOnly StartDate { get; private set; }
    public MembershipStatus Status { get; private set; }

    private Membership(MembershipId id, string userId, MembershipTypeId membershipTypeId, DateOnly startDate, MembershipStatus status)
    {
        Id = id;
        UserId = userId;
        MembershipTypeId = membershipTypeId;
        StartDate = startDate;
        Status = status;
    }

    private Membership() { }

    public static Membership Create(string userId, MembershipTypeId membershipTypeId, DateOnly startDate)
    {
        DomainValidator.RequiredString(userId, MembershipErrors.UserIdRequired);
        DomainValidator.RequiredGuid(membershipTypeId.Value, MembershipErrors.MembershipTypeIdRequired);

        if (startDate < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ValidationDomainException(MembershipErrors.StartDateCannotBeInThePast);

        return new Membership(
            MembershipId.Create(),
            userId,
            membershipTypeId,
            startDate,
            MembershipStatus.Active);
    }

    public void Cancel()
    {
        if (Status == MembershipStatus.Cancelled)
            throw new ValidationDomainException(MembershipErrors.MembershipAlreadyCancelled);

        Status = MembershipStatus.Cancelled;
    }

    public void Pause()
    {
        if (Status != MembershipStatus.Active)
            throw new ValidationDomainException(MembershipErrors.OnlyActiveMembershipCanBePaused);

        Status = MembershipStatus.Paused;
    }

    public void Activate()
    {
        if (Status == MembershipStatus.Active)
            throw new ValidationDomainException(MembershipErrors.MembershipAlreadyActive);

        Status = MembershipStatus.Active;
    }

    public bool IsActive()
    {
        return Status == MembershipStatus.Active;
    }
}