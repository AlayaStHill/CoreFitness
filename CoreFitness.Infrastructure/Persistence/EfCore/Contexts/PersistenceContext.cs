using CoreFitness.Application.Shared;
using CoreFitness.Domain.Aggregates.CustomerService;
using CoreFitness.Domain.Aggregates.Members;
using CoreFitness.Domain.Aggregates.Members.Memberships;
using CoreFitness.Domain.Aggregates.MembershipTypes;
using CoreFitness.Domain.Aggregates.MembershipTypes.MembershipBenefits;
using CoreFitness.Domain.Aggregates.WorkoutCategories;
using CoreFitness.Domain.Aggregates.WorkoutSessions;
using CoreFitness.Domain.Aggregates.WorkoutSessions.Bookings;
using CoreFitness.Domain.Aggregates.WorkoutTypes;
using CoreFitness.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreFitness.Infrastructure.Persistence.EfCore.Contexts;

public class PersistenceContext(DbContextOptions<PersistenceContext> options) : IdentityDbContext<ApplicationUser, IdentityRole, string>(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // identity använder 
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersistenceContext).Assembly);
    }

    //DbContext har redan en egen SaveChangesAsync. Implementerar IUnitOfWork genom att overridea DbContext.SaveChangesAsync och delegera till EF Core (base.SaveChangesAsync).
    public override Task<int> SaveChangesAsync(CancellationToken ct)
    => base.SaveChangesAsync(ct);

    public DbSet<ContactRequest> ContactRequests => Set<ContactRequest>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<MembershipType> MembershipTypes => Set<MembershipType>();
    public DbSet<WorkoutCategory> WorkoutCategories => Set<WorkoutCategory>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();
    public DbSet<WorkoutType> WorkoutTypes => Set<WorkoutType>();
    public DbSet<MembershipBenefit> MembershipBenefits => Set<MembershipBenefit>();
















    //public override async Task<int> SaveChangesAsync(CancellationToken ct)
    //{
    //    DateTime utcNow = DateTime.UtcNow;

    //    foreach (EntityEntry<EntityBase> entry in ChangeTracker.Entries<EntityBase>())
    //    {
    //        if (entry.State == EntityState.Added)
    //        {
    //            entry.Entity.CreatedAt = utcNow;
    //            entry.Entity.ModifiedAt = utcNow;
    //        }
    //        else if (entry.State == EntityState.Modified)
    //        {
    //            entry.Entity.ModifiedAt = utcNow;
    //        }
    //    }

    //    return await base.SaveChangesAsync(ct);
    //}

}

