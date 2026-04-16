using CoreFitness.Application.Members;
using CoreFitness.Application.MembershipTypes;
using CoreFitness.Application.MyAccount;
using CoreFitness.Application.Shared;
using NSubstitute;

namespace CoreFitness.Application.Tests.MyAccount;

internal sealed class MyAccountMembershipServiceFixture
{
    public IMemberRepository MemberRepository { get; } = Substitute.For<IMemberRepository>();
    public IMembershipTypeRepository MembershipTypeRepository { get; } = Substitute.For<IMembershipTypeRepository>();
    public IUnitOfWork UnitOfWork { get; } = Substitute.For<IUnitOfWork>();

    public MyAccountMembershipService CreateSut() => new(MemberRepository, MembershipTypeRepository, UnitOfWork);
}
