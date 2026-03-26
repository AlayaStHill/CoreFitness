using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Domain.Exceptions.Custom;

namespace CoreFitness.Domain.Aggregates.WorkoutSessions.Services;

public sealed class WorkoutBookingDomainService
{
    public void Book(Member member, WorkoutSession workoutSession)
    {
        if (!member.HasActiveMembership())
            throw new ValidationDomainException(MemberErrors.MemberHasNoMembership);

        workoutSession.Book(member.Id);
    }
}
