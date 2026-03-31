using CoreFitness.Application.MembershipTypes.Outputs;

namespace CoreFitness.Application.MembershipTypes;

public interface IMembershipTypeRepository
{
    Task<IReadOnlyCollection<MembershipTypeFeaturedOutput>> GetFeaturedAsync(CancellationToken ct);
}
