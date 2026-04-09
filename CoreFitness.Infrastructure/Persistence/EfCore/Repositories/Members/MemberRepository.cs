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
}
