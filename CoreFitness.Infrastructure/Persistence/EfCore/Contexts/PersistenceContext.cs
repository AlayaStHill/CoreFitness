using CoreFitness.Application.Shared;
using CoreFitness.Domain.Aggregates.CustomerService;
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

