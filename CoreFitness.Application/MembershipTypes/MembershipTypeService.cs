using CoreFitness.Application.MembershipTypes.Outputs;
using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.MembershipTypes;

public sealed class MembershipTypeService(IMembershipTypeRepository membershipTypes) : IMembershipTypeService
{
    public async Task<Result<IReadOnlyCollection<MembershipTypeFeaturedOutput>>> GetFeaturedMembershipTypesAsync(CancellationToken ct = default)
    {
        var featuredMembershipTypes = await membershipTypes.GetFeaturedAsync(ct);
        if (featuredMembershipTypes == null)
            return Result<IReadOnlyCollection<MembershipTypeFeaturedOutput>>.Fail(ErrorTypes.Error, "Something went wrong");

        return Result<IReadOnlyCollection<MembershipTypeFeaturedOutput>>.Success(featuredMembershipTypes);
    }
}
