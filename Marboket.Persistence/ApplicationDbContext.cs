using Marboket.Domain.Common;
using Marboket.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Marboket.Persistence;

public sealed class ApplicationDbContext(DbContextOptions options)
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{

    public DbSet<Category> Categories { get; set; }
    public DbSet<ItemUnit> ItemUnits { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<Price> Prices { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(PersistenceAssemblyReference.Assembly);
    }

    public override int SaveChanges()
    {
        var now = DateTime.UtcNow;

        foreach (var changedEntity in ChangeTracker.Entries())
        {
            if (changedEntity.Entity is IAuditableEntity entity)
            {
                switch (changedEntity.State)
                {
                    case EntityState.Modified:
                        entity.UpdateDate = now;
                        break;
                }
            }
        }
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        foreach (var changedEntity in ChangeTracker.Entries())
        {
            if (changedEntity.Entity is IAuditableEntity entity)
            {
                switch (changedEntity.State)
                {
                    case EntityState.Modified:
                        entity.UpdateDate = now;
                        break;
                }
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        foreach (var changedEntity in ChangeTracker.Entries())
        {
            if (changedEntity.Entity is IAuditableEntity entity)
            {
                switch (changedEntity.State)
                {
                    case EntityState.Modified:
                        entity.UpdateDate = now;
                        break;
                }
            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}

