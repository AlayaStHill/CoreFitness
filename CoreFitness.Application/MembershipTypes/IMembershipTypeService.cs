using CoreFitness.Application.MembershipTypes.Outputs;
using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.MembershipTypes;

public interface IMembershipTypeService
{
    Task<Result<IReadOnlyCollection<MembershipTypeFeaturedOutput>>> GetFeaturedMembershipTypesAsync(CancellationToken ct = default);
}
