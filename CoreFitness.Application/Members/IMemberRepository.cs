using CoreFitness.Domain.Abstractions.Repositories;
using CoreFitness.Domain.Aggregates.Members;

namespace CoreFitness.Application.Members;

public interface IMemberRepository : IRepositoryBase<Member, string>
{

}
