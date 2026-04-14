using CoreFitness.Application.CustomerService.ContatRequests;
using CoreFitness.Application.Tests.Common;
using NSubstitute;

namespace CoreFitness.Application.Tests.CustomerService.ContatRequests;

public sealed class ContactRequestServiceTests
{
    [Fact]
    public async Task CreateContactRequestAsync_ShouldFail_WhenInputIsNull()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.CreateContactRequestAsync(input: null!);

        ResultAssert.Failure(result, ContactRequestErrors.InputMustBeProvided);
        await fixture.Repository.DidNotReceive().AddAsync(Arg.Any<CoreFitness.Domain.Aggregates.CustomerService.ContactRequest>(), Arg.Any<CancellationToken>());
        await fixture.UnitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CreateContactRequestAsync_ShouldAddAndSave_WhenInputIsValid()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();
        var input = ApplicationTestData.CreateContactRequestInput();
        var addedRequest = ApplicationTestData.CreateContactRequest();

        fixture.Repository.AddAsync(Arg.Any<CoreFitness.Domain.Aggregates.CustomerService.ContactRequest>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(addedRequest));
        fixture.UnitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        var result = await sut.CreateContactRequestAsync(input);

        ResultAssert.Success(result);
        await fixture.Repository.Received(1).AddAsync(Arg.Any<CoreFitness.Domain.Aggregates.CustomerService.ContactRequest>(), Arg.Any<CancellationToken>());
        await fixture.UnitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteContactRequestAsync_ShouldFail_WhenIdIsMissing()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.DeleteContactRequestAsync(" ");

        ResultAssert.Failure(result, ContactRequestErrors.IdMustBeProvided);
    }

    [Fact]
    public async Task DeleteContactRequestAsync_ShouldFail_WhenRequestDoesNotExist()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();

        fixture.Repository.RemoveAsync("missing-id", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(false));

        var result = await sut.DeleteContactRequestAsync("missing-id");

        ResultAssert.Failure(result, ContactRequestErrors.NotFoundById("missing-id"));
        await fixture.UnitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteContactRequestAsync_ShouldSave_WhenRequestExists()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();

        fixture.Repository.RemoveAsync("id-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(true));
        fixture.UnitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        var result = await sut.DeleteContactRequestAsync("id-1");

        ResultAssert.Success(result);
        await fixture.UnitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task GetAllContactRequestsAsync_ShouldReturnSuccessWithData()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();
        var requests = new List<CoreFitness.Domain.Aggregates.CustomerService.ContactRequest>
        {
            ApplicationTestData.CreateContactRequest(),
            ApplicationTestData.CreateContactRequest(firstName: "Jane", email: "jane@doe.com")
        };

        fixture.Repository.GetAllAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<IReadOnlyList<CoreFitness.Domain.Aggregates.CustomerService.ContactRequest>>(requests));

        var result = await sut.GetAllContactRequestsAsync();

        ResultAssert.Success(result);
        Assert.Equal(2, result.Value.Count);
    }

    [Fact]
    public async Task GetContactRequestAsync_ShouldFail_WhenIdIsMissing()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.GetContactRequestAsync(" ");

        ResultAssert.Failure(result, ContactRequestErrors.ContactRequestIdMustBeProvided);
    }

    [Fact]
    public async Task GetContactRequestAsync_ShouldFail_WhenRequestNotFound()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();

        fixture.Repository.GetByIdAsync("id-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.CustomerService.ContactRequest?>(null));

        var result = await sut.GetContactRequestAsync("id-1");

        ResultAssert.Failure(result, ContactRequestErrors.NotFoundById("id-1"));
    }

    [Fact]
    public async Task GetContactRequestAsync_ShouldReturnSuccess_WhenRequestExists()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();
        var request = ApplicationTestData.CreateContactRequest();

        fixture.Repository.GetByIdAsync("id-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.CustomerService.ContactRequest?>(request));

        var result = await sut.GetContactRequestAsync("id-1");

        ResultAssert.Success(result);
        Assert.Equal(request, result.Value);
    }

    [Fact]
    public async Task MarkAsReadAsync_ShouldFail_WhenIdIsMissing()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();

        var result = await sut.MarkAsReadAsync(" ");

        ResultAssert.Failure(result, ContactRequestErrors.IdMustBeProvided);
    }

    [Fact]
    public async Task MarkAsReadAsync_ShouldSave_WhenRequestWasUnread()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();
        var request = ApplicationTestData.CreateContactRequest();

        fixture.Repository.GetByIdAsync("id-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.CustomerService.ContactRequest?>(request));
        fixture.UnitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        var result = await sut.MarkAsReadAsync("id-1");

        ResultAssert.Success(result);
        Assert.True(request.MarkedAsRead);
        await fixture.UnitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task MarkAsReadAsync_ShouldNotSave_WhenRequestAlreadyRead()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();
        var request = ApplicationTestData.CreateContactRequest();
        request.MarkAsRead();

        fixture.Repository.GetByIdAsync("id-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.CustomerService.ContactRequest?>(request));

        var result = await sut.MarkAsReadAsync("id-1");

        ResultAssert.Success(result);
        await fixture.UnitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task MarkAsUnreadAsync_ShouldSave_WhenRequestWasRead()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();
        var request = ApplicationTestData.CreateContactRequest();
        request.MarkAsRead();

        fixture.Repository.GetByIdAsync("id-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.CustomerService.ContactRequest?>(request));
        fixture.UnitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        var result = await sut.MarkAsUnreadAsync("id-1");

        ResultAssert.Success(result);
        Assert.False(request.MarkedAsRead);
        await fixture.UnitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task MarkAsUnreadAsync_ShouldNotSave_WhenRequestAlreadyUnread()
    {
        var fixture = new ContactRequestServiceFixture();
        var sut = fixture.CreateSut();
        var request = ApplicationTestData.CreateContactRequest();

        fixture.Repository.GetByIdAsync("id-1", Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<CoreFitness.Domain.Aggregates.CustomerService.ContactRequest?>(request));

        var result = await sut.MarkAsUnreadAsync("id-1");

        ResultAssert.Success(result);
        await fixture.UnitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
