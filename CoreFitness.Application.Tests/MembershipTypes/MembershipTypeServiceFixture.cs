using CoreFitness.Application.MembershipTypes;
using NSubstitute;

namespace CoreFitness.Application.Tests.MembershipTypes;

internal sealed class MembershipTypeServiceFixture
{
    public IMembershipTypeRepository Repository { get; } = Substitute.For<IMembershipTypeRepository>();

    public MembershipTypeService CreateSut() => new(Repository);
}
