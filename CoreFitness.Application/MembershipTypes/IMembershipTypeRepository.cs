using CoreFitness.Application.MembershipTypes.Outputs;

namespace CoreFitness.Application.MembershipTypes;

public interface IMembershipTypeRepository
{
    Task<IReadOnlyList<MembershipTypeFeaturedOutput>> GetFeaturedAsync(CancellationToken ct);
}
