using corefitness.domain.shared.validators;
using CoreFitness.Domain.Aggregates.Members.Memberships;
using CoreFitness.Domain.Aggregates.MembershipTypes;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Domain.Aggregates.Members;

public sealed class Member
{
    private readonly List<Membership> _memberships = [];

    public MemberId Id { get; private set; } = default!;
    public string IdentityUserId { get; private set; } = null!;

    public IReadOnlyCollection<Membership> Memberships => _memberships.AsReadOnly();

    private Member(MemberId id, string identityUserId)
    {
        Id = id;
        IdentityUserId = identityUserId;
    }

    private Member() { }

    public static Member Create(string identityUserId)
    {
        DomainValidator.RequiredString(identityUserId, MemberErrors.IdentityUserIdRequired);

        return new Member(MemberId.Create(), identityUserId);
    }

    public void StartMembership(MembershipTypeId membershipTypeId, DateOnly startDate)
    {
        if (_memberships.Any(x => x.IsActive()))
            throw new ValidationDomainException(MemberErrors.MemberAlreadyHasMembership);

        var membership = Membership.Create(Id, membershipTypeId, startDate);
        _memberships.Add(membership);
    }

    public void ChangeActiveMembershipType(MembershipTypeId membershipTypeId)
    {
        var activeMembership = GetActiveMembershipOrThrow();
        activeMembership.ChangeMembershipType(membershipTypeId);
    }

    public void PauseActiveMembership()
    {
        var activeMembership = GetActiveMembershipOrThrow();
        activeMembership.Pause();
    }

    public void ActivatePausedMembership()
    {
        var pausedMembership = _memberships.FirstOrDefault(x => x.IsPaused());

        if (pausedMembership is null)
            throw new ValidationDomainException(MemberErrors.MemberHasNoMembership);

        if (_memberships.Any(x => x.IsActive()))
            throw new ValidationDomainException(MemberErrors.MemberAlreadyHasMembership);

        pausedMembership.Activate();
    }

    public void CancelActiveMembership()
    {
        var activeMembership = GetActiveMembershipOrThrow();
        activeMembership.Cancel();
    }

    public bool HasActiveMembership()
    {
        return _memberships.Any(x => x.IsActive());
    }

    public Membership? GetActiveMembership()
    {
        return _memberships.FirstOrDefault(x => x.IsActive());
    }

    private Membership GetActiveMembershipOrThrow()
    {
        var membership = _memberships.FirstOrDefault(x => x.IsActive());

        if (membership is null)
            throw new ValidationDomainException(MemberErrors.MemberHasNoMembership);

        return membership;
    }
}