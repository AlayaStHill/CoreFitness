using CoreFitness.Application.MembershipTypes;
using CoreFitness.Application.MembershipTypes.Outputs;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Repositories.MembershipTypes;

public sealed class MembershipTypeRepository(PersistenceContext context) : IMembershipTypeRepository
{
    public async Task<IReadOnlyList<MembershipTypeFeaturedOutput>> GetFeaturedAsync(CancellationToken ct)
    {
        List<MembershipTypeFeaturedOutput> featured = await context.MembershipTypes
            .AsNoTracking()
            .Where(membershipType => membershipType.IsFeatured)
            .Select(membershipType => new MembershipTypeFeaturedOutput
            {
                Id = membershipType.Id.Value,
                Name = membershipType.Name,
                Description = membershipType.Description,
                ClassesPerMonth = membershipType.ClassesPerMonth,
                PricePerMonth = membershipType.PricePerMonth,
                Benefits = membershipType.Benefits
                    .Select(benefit => benefit.Text)
                    .ToArray()
            })
            .ToListAsync(ct);

        return featured
            .OrderBy(membershipType => membershipType.PricePerMonth)
            .ToList();
    }
}
