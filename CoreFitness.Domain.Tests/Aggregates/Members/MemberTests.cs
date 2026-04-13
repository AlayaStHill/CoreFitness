using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Domain.Aggregates.MembershipTypes;
using CoreFitness.Domain.Tests.Common;

namespace CoreFitness.Domain.Tests.Aggregates.Members;

public sealed class MemberTests
{
    [Fact]
    public void Create_ShouldThrow_WhenUserIdIsMissing()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => Member.Create("  "),
            MemberErrors.UserIdRequired);
    }

    [Fact]
    public void StartMembership_ShouldCreateActiveMembership_WhenNoActiveMembershipExists()
    {
        var member = Member.Create("user-1");

        member.StartMembership(MembershipTypeId.Create());

        Assert.True(member.HasActiveMembership());
        Assert.NotNull(member.GetActiveMembership());
    }

    [Fact]
    public void StartMembership_ShouldThrow_WhenMemberAlreadyHasActiveMembership()
    {
        var member = Member.Create("user-1");
        member.StartMembership(MembershipTypeId.Create());

        ValidationExceptionAssert.ThrowsWithMessage(
            () => member.StartMembership(MembershipTypeId.Create()),
            MemberErrors.MemberAlreadyHasActiveMembership);
    }

    [Fact]
    public void PauseActiveMembership_ShouldThrow_WhenNoActiveMembershipExists()
    {
        var member = Member.Create("user-1");

        ValidationExceptionAssert.ThrowsWithMessage(
            member.PauseActiveMembership,
            MemberErrors.MemberHasNoMembership);
    }

    [Fact]
    public void PauseAndActivatePausedMembership_ShouldRestoreActiveMembership()
    {
        var member = Member.Create("user-1");
        member.StartMembership(MembershipTypeId.Create());

        member.PauseActiveMembership();
        member.ActivatePausedMembership();

        Assert.True(member.HasActiveMembership());
        Assert.NotNull(member.GetActiveMembership());
    }

    [Fact]
    public void ChangeActiveMembershipType_ShouldThrow_WhenNoActiveMembershipExists()
    {
        var member = Member.Create("user-1");

        ValidationExceptionAssert.ThrowsWithMessage(
            () => member.ChangeActiveMembershipType(MembershipTypeId.Create()),
            MemberErrors.MemberHasNoMembership);
    }

    [Fact]
    public void CancelActiveMembership_ShouldThrow_WhenNoActiveMembershipExists()
    {
        var member = Member.Create("user-1");

        ValidationExceptionAssert.ThrowsWithMessage(
            member.CancelActiveMembership,
            MemberErrors.MemberHasNoMembership);
    }

    [Fact]
    public void ActivatePausedMembership_ShouldThrow_WhenNoPausedMembershipExists()
    {
        var member = Member.Create("user-1");

        ValidationExceptionAssert.ThrowsWithMessage(
            member.ActivatePausedMembership,
            MemberErrors.MemberHasNoPausedMembership);
    }

    [Fact]
    public void ActivatePausedMembership_ShouldThrow_WhenAnotherActiveMembershipExists()
    {
        var member = Member.Create("user-1");
        member.StartMembership(MembershipTypeId.Create());
        member.PauseActiveMembership();
        member.StartMembership(MembershipTypeId.Create());

        ValidationExceptionAssert.ThrowsWithMessage(
            member.ActivatePausedMembership,
            MemberErrors.MemberAlreadyHasActiveMembership);
    }

    [Fact]
    public void ChangeActiveMembershipType_ShouldUpdateMembershipType_WhenMembershipIsActive()
    {
        var member = Member.Create("user-1");
        var initialTypeId = MembershipTypeId.Create();
        var newTypeId = MembershipTypeId.Create();
        member.StartMembership(initialTypeId);

        member.ChangeActiveMembershipType(newTypeId);

        Assert.Equal(newTypeId, member.GetActiveMembership()!.MembershipTypeId);
    }

    [Fact]
    public void CancelActiveMembership_ShouldRemoveActiveMembershipState()
    {
        var member = Member.Create("user-1");
        member.StartMembership(MembershipTypeId.Create());

        member.CancelActiveMembership();

        Assert.False(member.HasActiveMembership());
        Assert.Null(member.GetActiveMembership());
    }
}
