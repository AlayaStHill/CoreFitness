using CoreFitness.Domain.Aggregates.MembershipTypes;
using CoreFitness.Domain.Aggregates.MembershipTypes.MembershipBenefits;
using CoreFitness.Domain.Tests.Common;

namespace CoreFitness.Domain.Tests.Aggregates.MembershipTypes;

public sealed class MembershipTypeTests
{
    [Fact]
    public void Create_ShouldThrow_WhenNameIsMissing()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => MembershipType.Create(" ", "Desc", 100m, 4),
            MembershipTypeErrors.NameRequired);
    }

    [Fact]
    public void Create_ShouldThrow_WhenDescriptionIsMissing()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => MembershipType.Create("Gold", " ", 100m, 4),
            MembershipTypeErrors.DescriptionRequired);
    }

    [Fact]
    public void Create_ShouldThrow_WhenPricePerMonthIsNegative()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => MembershipType.Create("Gold", "Desc", -1m, 4),
            MembershipTypeErrors.PriceMustBePositive);
    }

    [Fact]
    public void Create_ShouldSetProperties_WhenInputIsValid()
    {
        var membershipType = MembershipType.Create("Gold", "Best plan", 499m, 8, true);

        Assert.Equal("Gold", membershipType.Name);
        Assert.Equal("Best plan", membershipType.Description);
        Assert.Equal(499m, membershipType.PricePerMonth);
        Assert.Equal(8, membershipType.ClassesPerMonth);
        Assert.True(membershipType.IsFeatured);
    }

    [Fact]
    public void Create_ShouldThrow_WhenClassesPerMonthIsNegative()
    {
        ValidationExceptionAssert.ThrowsWithMessage(
            () => MembershipType.Create("Gold", "Desc", 100m, -1),
            MembershipTypeErrors.ClassesPerMonthMustBePositive);
    }

    [Fact]
    public void SetFeatured_ShouldUpdateIsFeaturedFlag()
    {
        var membershipType = MembershipType.Create("Gold", "Best plan", 499m, 8, false);

        membershipType.SetFeatured(true);
        Assert.True(membershipType.IsFeatured);

        membershipType.SetFeatured(false);
        Assert.False(membershipType.IsFeatured);
    }

    [Fact]
    public void AddBenefit_ShouldThrow_WhenBenefitTextAlreadyExists()
    {
        var membershipType = MembershipType.Create("Gold", "Best plan", 499m, 8);
        membershipType.AddBenefit("Sauna");

        ValidationExceptionAssert.ThrowsWithMessage(
            () => membershipType.AddBenefit("Sauna"),
            MembershipBenefitErrors.DuplicateBenefitText);
    }

    [Fact]
    public void UpdateBenefit_ShouldThrow_WhenBenefitDoesNotExist()
    {
        var membershipType = MembershipType.Create("Gold", "Best plan", 499m, 8);

        ValidationExceptionAssert.ThrowsWithMessage(
            () => membershipType.UpdateBenefit(MembershipBenefitId.Create(), "New text"),
            MembershipBenefitErrors.BenefitNotFound);
    }

    [Fact]
    public void RemoveBenefit_ShouldRemoveBenefit_WhenBenefitExists()
    {
        var membershipType = MembershipType.Create("Gold", "Best plan", 499m, 8);
        membershipType.AddBenefit("Sauna");
        var benefitId = membershipType.Benefits.Single().Id;

        membershipType.RemoveBenefit(benefitId);

        Assert.Empty(membershipType.Benefits);
    }

    [Fact]
    public void UpdateBenefit_ShouldThrow_WhenUpdatedTextDuplicatesAnotherBenefit()
    {
        var membershipType = MembershipType.Create("Gold", "Best plan", 499m, 8);
        membershipType.AddBenefit("Sauna");
        membershipType.AddBenefit("Pool");
        var poolBenefitId = membershipType.Benefits.Single(x => x.Text == "Pool").Id;

        ValidationExceptionAssert.ThrowsWithMessage(
            () => membershipType.UpdateBenefit(poolBenefitId, "Sauna"),
            MembershipBenefitErrors.DuplicateBenefitText);
    }
}
