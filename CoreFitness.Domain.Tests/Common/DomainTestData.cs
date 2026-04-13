using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Domain.Aggregates.MembershipTypes;
using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Domain.Aggregates.WorkoutTypes;

namespace CoreFitness.Domain.Tests.Common;

internal static class DomainTestData
{
    public static Member CreateMemberWithActiveMembership(string userId = "user-1")
    {
        var member = Member.Create(userId);
        member.StartMembership(MembershipTypeId.Create());
        return member;
    }

    public static WorkoutSession CreateFutureWorkoutSession(int capacity = 10)
        => WorkoutSession.Create(
            DateTimeOffset.UtcNow.AddHours(2),
            WorkoutTypeId.Create(),
            TimeSpan.FromMinutes(45),
            capacity);
}
