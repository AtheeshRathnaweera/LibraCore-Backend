using LibraCore.Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraCore.Backend.Data;

public class MainDBContext : DbContext
{
  public MainDBContext(DbContextOptions<MainDBContext> context) : base(context)
  {
  }

  public required DbSet<RoleEntity> Role { get; set; }

  // protected override void OnModelCreating(ModelBuilder modelBuilder)
  // {
  //   base.OnModelCreating(modelBuilder);

  //   modelBuilder.Entity<RoleEntity>(entity =>
  //   {
  //     entity.Property(r => r.CreatedAt).HasColumnName("created_at");
  //     entity.Property(r => r.UpdatedAt).HasColumnName("updated_at");
  //   });
  // }
}