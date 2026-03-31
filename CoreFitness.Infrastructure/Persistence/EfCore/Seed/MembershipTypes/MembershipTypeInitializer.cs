using CoreFitness.Domain.Aggregates.MembershipTypes;
using CoreFitness.Infrastructure.Persistence.EfCore.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Seed.MembershipTypes;

public static class MembershipTypeInitializer
{
    public static async Task InitializeDefaultAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<PersistenceContext>();

        if (!await dbContext.MembershipTypes.AnyAsync(x => x.Name == "Standard Membership"))
        {
            var standardMembership = MembershipType.Create(
                "Standard Membership",
                "With the Standard Membership, get access to our full range of gym facilities.",
                495,
                20);

            standardMembership.AddBenefit("Standard Locker");
            standardMembership.AddBenefit("High-energy group fitness classes");
            standardMembership.AddBenefit("Motivating & supportive environment");

            dbContext.MembershipTypes.Add(standardMembership);
        }

        if (!await dbContext.MembershipTypes.AnyAsync(x => x.Name == "Premium Membership"))
        {
            var premiumMembership = MembershipType.Create(
                "Premium Membership",
                "With the Premium Membership, get access to our full range of gym facilities.",
                595,
                20);

            premiumMembership.AddBenefit("Priority Support & Premium Locker");
            premiumMembership.AddBenefit("High-energy group fitness classes");
            premiumMembership.AddBenefit("Motivating & supportive environment");

            dbContext.MembershipTypes.Add(premiumMembership);
        }

        await dbContext.SaveChangesAsync(CancellationToken.None);
    }
}