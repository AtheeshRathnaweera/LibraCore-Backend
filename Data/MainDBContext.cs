using LibraCore.Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraCore.Backend.Data;

public class MainDBContext : DbContext
{
  public MainDBContext(DbContextOptions<MainDBContext> context) : base(context)
  {
  }

  public required DbSet<RoleEntity> Role { get; set; }
  public required DbSet<UserStatusEntity> UserStatus { get; set; }
  public required DbSet<UserEntity> User { get; set; }
}