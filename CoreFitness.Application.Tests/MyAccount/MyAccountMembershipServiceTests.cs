using CoreFitness.Application.MyAccount;
using CoreFitness.Application.Tests.Common;
using NSubstitute;

namespace CoreFitness.Application.Tests.MyAccount;

public sealed class MyAccountMembershipServiceTests
{
    [Fact]
    public async Task GetOverviewAsync_ShouldReturnNull_WhenUserIdIsMissing()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.GetOverviewAsync(" ");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetOverviewAsync_ShouldReturnNull_WhenMemberIsNotFound()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(null));

        var result = await sut.GetOverviewAsync("user-1");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetOverviewAsync_ShouldReturnPlansWithoutCurrentMembership_WhenMemberHasNoMembership()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMember();
        var plans = new List<CoreFitness.Application.MembershipTypes.Outputs.MembershipTypeFeaturedOutput>
        {
            ApplicationTestData.CreateMembershipTypeFeaturedOutput(),
            ApplicationTestData.CreateMembershipTypeFeaturedOutput(name: "Silver", pricePerMonth: 299m)
        };

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.MembershipTypeRepository.GetFeaturedAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<CoreFitness.Application.MembershipTypes.Outputs.MembershipTypeFeaturedOutput>>(plans));

        var result = await sut.GetOverviewAsync("user-1");

        Assert.NotNull(result);
        Assert.False(result.HasMembership);
        Assert.Null(result.CurrentMembership);
        Assert.Equal(2, result.AvailablePlans.Count);
    }

    [Fact]
    public async Task GetOverviewAsync_ShouldReturnCurrentMembership_WhenActiveMembershipExists()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMemberWithActiveMembership();
        var activeMembership = member.GetActiveMembership()!;
        var plans = new List<CoreFitness.Application.MembershipTypes.Outputs.MembershipTypeFeaturedOutput>
        {
            ApplicationTestData.CreateMembershipTypeFeaturedOutput(id: activeMembership.MembershipTypeId.Value, name: "Gold")
        };

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.MembershipTypeRepository.GetFeaturedAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<CoreFitness.Application.MembershipTypes.Outputs.MembershipTypeFeaturedOutput>>(plans));

        var result = await sut.GetOverviewAsync("user-1");

        Assert.NotNull(result);
        Assert.True(result.HasMembership);
        Assert.NotNull(result.CurrentMembership);
        Assert.Equal("Gold", result.CurrentMembership!.MembershipName);
    }

    [Fact]
    public async Task SelectPlanAsync_ShouldFail_WhenUserIdIsMissing()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.SelectPlanAsync(" ", Guid.NewGuid());

        ResultAssert.Failure(result, MyAccountErrors.UserIdRequired);
    }

    [Fact]
    public async Task SelectPlanAsync_ShouldFail_WhenMembershipTypeIdIsEmpty()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.SelectPlanAsync("user-1", Guid.Empty);

        ResultAssert.Failure(result, MyAccountErrors.MembershipTypeIdRequired);
    }

    [Fact]
    public async Task SelectPlanAsync_ShouldFail_WhenMemberIsNotFound()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(null));

        var result = await sut.SelectPlanAsync("user-1", Guid.NewGuid());

        ResultAssert.Failure(result, MyAccountErrors.MemberNotFound);
    }

    [Fact]
    public async Task SelectPlanAsync_ShouldFail_WhenPlanIsNotFound()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMember();

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.MembershipTypeRepository.GetFeaturedAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<CoreFitness.Application.MembershipTypes.Outputs.MembershipTypeFeaturedOutput>>(
                new List<CoreFitness.Application.MembershipTypes.Outputs.MembershipTypeFeaturedOutput>()));

        var result = await sut.SelectPlanAsync("user-1", Guid.NewGuid());

        ResultAssert.Failure(result, MyAccountErrors.SelectedMembershipPlanNotFound);
    }

    [Fact]
    public async Task SelectPlanAsync_ShouldFailWithValidation_WhenMemberAlreadyHasActiveMembership()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMemberWithActiveMembership();
        var selectedId = Guid.NewGuid();
        var plans = new List<CoreFitness.Application.MembershipTypes.Outputs.MembershipTypeFeaturedOutput>
        {
            ApplicationTestData.CreateMembershipTypeFeaturedOutput(id: selectedId)
        };

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.MembershipTypeRepository.GetFeaturedAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<CoreFitness.Application.MembershipTypes.Outputs.MembershipTypeFeaturedOutput>>(plans));

        var result = await sut.SelectPlanAsync("user-1", selectedId);

        Assert.True(result.IsFailure);
        Assert.Equal(CoreFitness.Application.Shared.Results.ErrorTypes.BadRequest, result.Error!.Type);
        await fixture.UnitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SelectPlanAsync_ShouldSave_WhenPlanIsValid()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMember();
        var selectedId = Guid.NewGuid();
        var plans = new List<CoreFitness.Application.MembershipTypes.Outputs.MembershipTypeFeaturedOutput>
        {
            ApplicationTestData.CreateMembershipTypeFeaturedOutput(id: selectedId)
        };

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.MembershipTypeRepository.GetFeaturedAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<CoreFitness.Application.MembershipTypes.Outputs.MembershipTypeFeaturedOutput>>(plans));
        fixture.UnitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        var result = await sut.SelectPlanAsync("user-1", selectedId);

        ResultAssert.Success(result);
        await fixture.UnitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CancelActiveMembershipAsync_ShouldFail_WhenUserIdIsMissing()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.CancelActiveMembershipAsync(" ");

        ResultAssert.Failure(result, MyAccountErrors.UserIdRequired);
    }

    [Fact]
    public async Task CancelActiveMembershipAsync_ShouldFail_WhenMemberIsNotFound()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(null));

        var result = await sut.CancelActiveMembershipAsync("user-1");

        ResultAssert.Failure(result, MyAccountErrors.MemberNotFound);
    }

    [Fact]
    public async Task CancelActiveMembershipAsync_ShouldSave_WhenMemberHasActiveMembership()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMemberWithActiveMembership();

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.UnitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        var result = await sut.CancelActiveMembershipAsync("user-1");

        ResultAssert.Success(result);
        await fixture.UnitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CancelActiveMembershipAsync_ShouldFailWithValidation_WhenMemberHasNoMemberships()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMember();

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));

        var result = await sut.CancelActiveMembershipAsync("user-1");

        Assert.True(result.IsFailure);
        Assert.Equal(CoreFitness.Application.Shared.Results.ErrorTypes.BadRequest, result.Error!.Type);
    }

    [Fact]
    public async Task PauseActiveMembershipAsync_ShouldFail_WhenUserIdIsMissing()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.PauseActiveMembershipAsync(" ");

        ResultAssert.Failure(result, MyAccountErrors.UserIdRequired);
    }

    [Fact]
    public async Task PauseActiveMembershipAsync_ShouldFail_WhenMemberIsNotFound()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(null));

        var result = await sut.PauseActiveMembershipAsync("user-1");

        ResultAssert.Failure(result, MyAccountErrors.MemberNotFound);
    }

    [Fact]
    public async Task PauseActiveMembershipAsync_ShouldSave_WhenMemberHasActiveMembership()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMemberWithActiveMembership();

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.UnitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        var result = await sut.PauseActiveMembershipAsync("user-1");

        ResultAssert.Success(result);
        await fixture.UnitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task PauseActiveMembershipAsync_ShouldFailWithValidation_WhenMemberHasNoActiveMembership()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMember();

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));

        var result = await sut.PauseActiveMembershipAsync("user-1");

        Assert.True(result.IsFailure);
        Assert.Equal(CoreFitness.Application.Shared.Results.ErrorTypes.BadRequest, result.Error!.Type);
    }

    [Fact]
    public async Task ActivatePausedMembershipAsync_ShouldFail_WhenUserIdIsMissing()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.ActivatePausedMembershipAsync(" ");

        ResultAssert.Failure(result, MyAccountErrors.UserIdRequired);
    }

    [Fact]
    public async Task ActivatePausedMembershipAsync_ShouldFail_WhenMemberIsNotFound()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(null));

        var result = await sut.ActivatePausedMembershipAsync("user-1");

        ResultAssert.Failure(result, MyAccountErrors.MemberNotFound);
    }

    [Fact]
    public async Task ActivatePausedMembershipAsync_ShouldSave_WhenMemberHasPausedMembership()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMemberWithPausedMembership();

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));
        fixture.UnitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        var result = await sut.ActivatePausedMembershipAsync("user-1");

        ResultAssert.Success(result);
        await fixture.UnitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ActivatePausedMembershipAsync_ShouldFailWithValidation_WhenMemberHasNoPausedMembership()
    {
        var fixture = new MyAccountMembershipServiceFixture();
        var sut = fixture.CreateSut();
        var member = ApplicationTestData.CreateMemberWithActiveMembership();

        fixture.MemberRepository.GetByUserIdWithMembershipsAsync("user-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.Members.Member?>(member));

        var result = await sut.ActivatePausedMembershipAsync("user-1");

        Assert.True(result.IsFailure);
        Assert.Equal(CoreFitness.Application.Shared.Results.ErrorTypes.BadRequest, result.Error!.Type);
    }
}
