using CoreFitness.Application.MembershipTypes;
using CoreFitness.Application.Shared.Results;
using CoreFitness.Application.Tests.Common;
using NSubstitute;

namespace CoreFitness.Application.Tests.MembershipTypes;

public sealed class MembershipTypeServiceTests
{
    [Fact]
    public async Task GetFeaturedMembershipTypesAsync_ShouldFail_WhenRepositoryReturnsNull()
    {
        var fixture = new MembershipTypeServiceFixture();
        var sut = fixture.CreateSut();

        fixture.Repository.GetFeaturedAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<CoreFitness.Application.MembershipTypes.Outputs.MembershipTypeFeaturedOutput>>(null!));

        var result = await sut.GetFeaturedMembershipTypesAsync();

        ResultAssert.Failure(result, MembershipTypeErrors.SomethingWentWrong);
    }

    [Fact]
    public async Task GetFeaturedMembershipTypesAsync_ShouldReturnSuccess_WhenRepositoryReturnsList()
    {
        var fixture = new MembershipTypeServiceFixture();
        var sut = fixture.CreateSut();
        var featured = new List<CoreFitness.Application.MembershipTypes.Outputs.MembershipTypeFeaturedOutput>
        {
            ApplicationTestData.CreateMembershipTypeFeaturedOutput(),
            ApplicationTestData.CreateMembershipTypeFeaturedOutput(name: "Silver", pricePerMonth: 299m)
        };

        fixture.Repository.GetFeaturedAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<CoreFitness.Application.MembershipTypes.Outputs.MembershipTypeFeaturedOutput>>(featured));

        var result = await sut.GetFeaturedMembershipTypesAsync();

        ResultAssert.Success(result);
        Assert.Equal(2, result.Value.Count);
    }
}
