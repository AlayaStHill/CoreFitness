using CoreFitness.Application.MembershipTypes.Outputs;

namespace CoreFitness.Presentation.WebApp.Models.Memberships;

public sealed class MembershipsIndexViewModel
{
    public IReadOnlyCollection<MembershipTypeFeaturedOutput> Memberships { get; init; } = [];
}
