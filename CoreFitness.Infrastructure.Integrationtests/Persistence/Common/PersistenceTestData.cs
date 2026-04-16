using CoreFitness.Domain.Aggregates.MembershipTypes;
using CoreFitness.Domain.Aggregates.CustomerService;
using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Domain.Aggregates.WorkoutCategories;
using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Domain.Aggregates.WorkoutTypes;
using CoreFitness.Infrastructure.Identity.Models;
using System.Reflection;

namespace CoreFitness.Infrastructure.IntegrationTests.Persistence.Common;

internal static class PersistenceTestData
{
    private static readonly MethodInfo WorkoutSessionBookMethod =
        typeof(WorkoutSession).GetMethod("Book", BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new InvalidOperationException("Could not find WorkoutSession.Book method.");

    public static ApplicationUser CreateApplicationUser(string id, string email = "user@corefitness.test")
        => new()
        {
            Id = id,
            UserName = email,
            Email = email
        };

    public static Member CreateMember(string userId = "user-1")
        => Member.Create(userId);

    public static Member CreateMemberWithActiveMembership(MembershipTypeId membershipTypeId, string userId = "user-1")
    {
        var member = Member.Create(userId);
        member.StartMembership(membershipTypeId);
        return member;
    }

    public static ContactRequest CreateContactRequest(
        string firstName = "John",
        string lastName = "Doe",
        string email = "john@doe.com",
        string? phoneNumber = "0701234567",
        string message = "Hej detta är ett meddelande")
        => ContactRequest.Create(firstName, lastName, email, phoneNumber, message);

    public static MembershipType CreateMembershipType(
        string name,
        decimal pricePerMonth,
        bool isFeatured,
        int classesPerMonth = 8,
        params string[] benefits)
    {
        var membershipType = MembershipType.Create(
            name,
            $"{name} description",
            pricePerMonth,
            classesPerMonth,
            isFeatured);

        foreach (var benefit in benefits)
        {
            membershipType.AddBenefit(benefit);
        }

        return membershipType;
    }

    public static WorkoutCategory CreateWorkoutCategory(string title = "Mind & Body")
        => WorkoutCategory.Create(title);

    public static WorkoutType CreateWorkoutType(WorkoutCategoryId workoutCategoryId, string title = "Yoga")
        => WorkoutType.Create(title, workoutCategoryId);

    public static WorkoutSession CreateWorkoutSession(
        WorkoutTypeId workoutTypeId,
        DateTimeOffset? startsAt = null,
        TimeSpan? duration = null,
        int capacity = 12)
        => WorkoutSession.Create(
            startsAt ?? DateTimeOffset.UtcNow.AddHours(2),
            workoutTypeId,
            duration ?? TimeSpan.FromMinutes(45),
            capacity);

    public static void BookWorkoutSession(WorkoutSession workoutSession, MemberId memberId)
        => WorkoutSessionBookMethod.Invoke(workoutSession, [memberId]);
}
