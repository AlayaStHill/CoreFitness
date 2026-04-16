using CoreFitness.Application.CustomerService.ContatRequests;
using CoreFitness.Application.Shared;
using NSubstitute;

namespace CoreFitness.Application.Tests.CustomerService.ContatRequests;

internal sealed class ContactRequestServiceFixture
{
    public IContactRequestRepository Repository { get; } = Substitute.For<IContactRequestRepository>();
    public IUnitOfWork UnitOfWork { get; } = Substitute.For<IUnitOfWork>();

    public ContactRequestService CreateSut() => new(Repository, UnitOfWork);
}
