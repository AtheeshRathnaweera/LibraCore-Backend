using LibraCore.Backend.Entities;
using LibraCore.Backend.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace LibraCore.Backend.Data;

public class MainDBContext : DbContext
{
  // DbSet properties for the entities
  public required DbSet<RoleEntity> Role { get; set; }

  public required DbSet<UserStatusEntity> UserStatus { get; set; }

  public required DbSet<UserEntity> User { get; set; }

  public required DbSet<UserActiveStatusEntity> UserActiveStatus { get; set; }

  public MainDBContext(DbContextOptions<MainDBContext> context) : base(context)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // Apply CreatedAt and UpdatedAt default values for all entities
    foreach (var entityType in modelBuilder.Model.GetEntityTypes())
    {
      var createdAtProperty = entityType.FindProperty("CreatedAt");
      if (createdAtProperty != null && createdAtProperty.ClrType == typeof(DateTime))
      {
        createdAtProperty.SetDefaultValueSql("CURRENT_TIMESTAMP");
      }

      var updatedAtProperty = entityType.FindProperty("UpdatedAt");
      if (updatedAtProperty != null && updatedAtProperty.ClrType == typeof(DateTime?))
      {
        updatedAtProperty.SetDefaultValueSql("CURRENT_TIMESTAMP");
      }
    }
  }

  public override int SaveChanges()
  {
    UpdateTimestamps();
    return base.SaveChanges();
  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
    UpdateTimestamps();
    return await base.SaveChangesAsync(cancellationToken);
  }

  // Updates the CreatedAt and UpdatedAt timestamps for the entities.
  private void UpdateTimestamps()
  {
    var entries = ChangeTracker.Entries()
        .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

    foreach (var entry in entries)
    {
      if (entry.State == EntityState.Added)
      {
        ((BaseEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
      }

        ((BaseEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
    }
  }
}