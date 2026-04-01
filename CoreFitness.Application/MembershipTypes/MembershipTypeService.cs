using CoreFitness.Application.MembershipTypes.Outputs;
using CoreFitness.Application.Shared.Results;

namespace CoreFitness.Application.MembershipTypes;

public sealed class MembershipTypeService(IMembershipTypeRepository membershipTypes) : IMembershipTypeService
{
    public async Task<Result<IReadOnlyList<MembershipTypeFeaturedOutput>>> GetFeaturedMembershipTypesAsync(CancellationToken ct = default)
    {
        var featuredMembershipTypes = await membershipTypes.GetFeaturedAsync(ct);
        if (featuredMembershipTypes == null)
            return Result<IReadOnlyList<MembershipTypeFeaturedOutput>>.Fail(ErrorTypes.Error, "Something went wrong");

        return Result<IReadOnlyList<MembershipTypeFeaturedOutput>>.Success(featuredMembershipTypes);
    }
}
