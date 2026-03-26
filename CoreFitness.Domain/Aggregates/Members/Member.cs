using corefitness.domain.shared.validators;
using CoreFitness.Domain.Aggregates.Members.Memberships;
using CoreFitness.Domain.Aggregates.MembershipTypes;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Domain.Aggregates.Members;

public sealed class Member
{
    private readonly List<Membership> _memberships = [];

    public MemberId Id { get; private set; } = default!;
    public string UserId { get; private set; } = null!;

    public IReadOnlyCollection<Membership> Memberships => _memberships.AsReadOnly();

    private Member(MemberId id, string userId)
    {
        Id = id;
        UserId = userId;
    }

    private Member() { }

    public static Member Create(string userId)
    {
        DomainValidator.RequiredString(userId, MemberErrors.UserIdRequired);

        return new Member(MemberId.Create(), userId);
    }

    public void StartMembership(MembershipTypeId membershipTypeId)
    {
        if (_memberships.Any(x => x.IsActive()))
            throw new ValidationDomainException(MemberErrors.MemberAlreadyHasActiveMembership);

        var membership = Membership.Create(Id, membershipTypeId);
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
        var pausedMembership = GetPausedMembershipOrThrow();

        if (_memberships.Any(x => x.IsActive()))
            throw new ValidationDomainException(MemberErrors.MemberAlreadyHasActiveMembership);

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

    private Membership GetPausedMembershipOrThrow()
    {
        var membership = _memberships.FirstOrDefault(x => x.IsPaused());

        if (membership is null)
            throw new ValidationDomainException(MemberErrors.MemberHasNoPausedMembership);

        return membership;
    }
}