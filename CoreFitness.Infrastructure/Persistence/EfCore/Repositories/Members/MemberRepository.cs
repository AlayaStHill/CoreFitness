using CoreFitness.Application.Members;
using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Repositories.Members;

public sealed class MemberRepository(PersistenceContext context) : RepositoryBase<Member, string, PersistenceContext>(context), IMemberRepository
{
    public async Task<Member?> GetByUserIdWithMembershipsAsync(string userId, CancellationToken ct = default)
    {
        return await context.Members
            .Include(member => member.Memberships)
            .FirstOrDefaultAsync(member => member.UserId == userId, ct);
    }

    public async Task<bool> RemoveByUserIdAsync(string userId, CancellationToken ct = default)
    {
        var member = await context.Members
            .Include(x => x.Memberships)
            .FirstOrDefaultAsync(x => x.UserId == userId, ct);

        if (member is null)
            return false;

        var memberBookings = await context.Bookings
            .Where(booking => booking.MemberId == member.Id)
            .ToListAsync(ct);

        if (memberBookings.Count > 0)
            context.Bookings.RemoveRange(memberBookings);

        if (member.Memberships.Count > 0)
            context.Memberships.RemoveRange(member.Memberships);

        context.Members.Remove(member);
        return true;
    }
}
