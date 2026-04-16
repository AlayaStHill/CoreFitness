using corefitness.domain.shared.validators;
using CoreFitness.Domain.Aggregates.MembershipTypes;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Domain.Aggregates.Members.Memberships;

public sealed class Membership
{
    public MembershipId Id { get; private set; } = default!;
    public MemberId MemberId { get; private set; } = default!;
    public MembershipTypeId MembershipTypeId { get; private set; } = default!;
    public DateOnly StartDate { get; private set; }
    public MembershipStatus Status { get; private set; }

    private Membership(
        MembershipId id,
        MemberId memberId,
        MembershipTypeId membershipTypeId,
        DateOnly startDate,
        MembershipStatus status)
    {
        Id = id;
        MemberId = memberId;
        MembershipTypeId = membershipTypeId;
        StartDate = startDate;
        Status = status;
    }

    private Membership() { }

    internal static Membership Create(MemberId memberId, MembershipTypeId membershipTypeId)
    {
        DomainValidator.RequiredGuid(memberId.Value, MembershipErrors.MemberIdRequired);
        DomainValidator.RequiredGuid(membershipTypeId.Value, MembershipErrors.MembershipTypeIdRequired);

        return new Membership(
            MembershipId.Create(),
            memberId,
            membershipTypeId,
            DateOnly.FromDateTime(DateTime.UtcNow),
            MembershipStatus.Active);
    }

    internal void ChangeMembershipType(MembershipTypeId membershipTypeId)
    {
        DomainValidator.RequiredGuid(membershipTypeId.Value, MembershipErrors.MembershipTypeIdRequired);

        if (Status == MembershipStatus.Cancelled)
            throw new ValidationDomainException(MembershipErrors.CancelledMembershipCannotBeChanged);

        if (MembershipTypeId == membershipTypeId)
            throw new ValidationDomainException(MembershipErrors.MembershipTypeIsAlreadySelected);

        MembershipTypeId = membershipTypeId;
    }

    internal void Pause()
    {
        if (Status != MembershipStatus.Active)
            throw new ValidationDomainException(MembershipErrors.OnlyActiveMembershipCanBePaused);

        Status = MembershipStatus.Paused;
    }

    internal void Activate()
    {
        if (Status == MembershipStatus.Active)
            throw new ValidationDomainException(MembershipErrors.MembershipAlreadyActive);

        if (Status == MembershipStatus.Cancelled)
            throw new ValidationDomainException(MembershipErrors.CancelledMembershipCannotBeActivated);

        Status = MembershipStatus.Active;
    }

    internal void Cancel()
    {
        if (Status == MembershipStatus.Cancelled)
            throw new ValidationDomainException(MembershipErrors.MembershipAlreadyCancelled);

        Status = MembershipStatus.Cancelled;
    }

    public bool IsActive() => Status == MembershipStatus.Active;
    public bool IsPaused() => Status == MembershipStatus.Paused;
    public bool IsCancelled() => Status == MembershipStatus.Cancelled;
}