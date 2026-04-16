using CoreFitness.Application.CustomerService.ContatRequests.Inputs;
using CoreFitness.Application.MembershipTypes.Outputs;
using CoreFitness.Application.MyAccount.Outputs;
using CoreFitness.Domain.Aggregates.CustomerService;
using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Domain.Aggregates.MembershipTypes;
using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Domain.Aggregates.WorkoutTypes;

namespace CoreFitness.Application.Tests.Common;

internal static class ApplicationTestData
{
    public static ContactRequestInput CreateContactRequestInput(
        string firstName = "John",
        string lastName = "Doe",
        string email = "john@doe.com",
        string? phoneNumber = "0701234567",
        string message = "Hej detta är ett meddelande")
        => new(firstName, lastName, email, phoneNumber, message);

    public static ContactRequest CreateContactRequest(
        string firstName = "John",
        string lastName = "Doe",
        string email = "john@doe.com",
        string? phoneNumber = "0701234567",
        string message = "Hej detta är ett meddelande")
        => ContactRequest.Create(firstName, lastName, email, phoneNumber, message);

    public static MembershipTypeFeaturedOutput CreateMembershipTypeFeaturedOutput(
        Guid? id = null,
        string name = "Gold",
        decimal pricePerMonth = 499m,
        IReadOnlyList<string>? benefits = null)
        => new()
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
            Description = "Plan description",
            PricePerMonth = pricePerMonth,
            ClassesPerMonth = 8,
            Benefits = benefits ?? ["Gym", "Sauna"]
        };

    public static MyBookingItemOutput CreateMyBookingItemOutput(DateTimeOffset startsAt)
        => new(
            Guid.NewGuid(),
            "Yoga",
            "Mind & Body",
            startsAt,
            45);

    public static MyUpcomingSessionOutput CreateMyUpcomingSessionOutput(DateTimeOffset startsAt)
        => new(
            Guid.NewGuid(),
            "Yoga",
            "Mind & Body",
            startsAt,
            45,
            4,
            20,
            false);

    public static Member CreateMember(string userId = "user-1")
        => Member.Create(userId);

    public static Member CreateMemberWithActiveMembership(string userId = "user-1")
    {
        var member = CreateMember(userId);
        member.StartMembership(MembershipTypeId.Create());
        return member;
    }

    public static Member CreateMemberWithPausedMembership(string userId = "user-1")
    {
        var member = CreateMemberWithActiveMembership(userId);
        member.PauseActiveMembership();
        return member;
    }

    public static WorkoutSession CreateFutureWorkoutSession(int capacity = 10)
        => WorkoutSession.Create(
            DateTimeOffset.UtcNow.AddHours(2),
            WorkoutTypeId.Create(),
            TimeSpan.FromMinutes(45),
            capacity);
}
