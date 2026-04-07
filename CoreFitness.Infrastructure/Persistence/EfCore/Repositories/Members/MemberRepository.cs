using CoreFitness.Application.Members;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using CoreFitness.Domain.Aggregates.Members;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Repositories.Members;

public sealed class MemberRepository(PersistenceContext context) : RepositoryBase<Member, string, PersistenceContext>(context), IMemberRepository
{
}
