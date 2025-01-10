using LibraCore.Backend.Data;
using LibraCore.Backend.Entities;
using LibraCore.Backend.Models;
using LibraCore.Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraCore.Backend.Services.Implementations;

public class RoleService : IRoleService
{
  private readonly MainDBContext _dbContext;

  public RoleService(MainDBContext mainDBContext)
  {
    _dbContext = mainDBContext ?? throw new ArgumentNullException(nameof(mainDBContext));
  }

  public async Task<RoleModel?> GetAsync(int id)
  {
    var roleEntity = await _dbContext.Role.FindAsync(id);
    return roleEntity != null ? MapEntityToModel(roleEntity) : null;
  }

  public async Task<IEnumerable<RoleModel>> GetAllAsync()
  {
    var roles = await _dbContext.Role.ToListAsync();
    return roles.Select(MapEntityToModel);
  }

  public async Task<RoleModel> CreateAsync(RoleModel roleModel)
  {
    RoleEntity roleEntity = new()
    {
      Name = roleModel.Name ?? throw new ArgumentNullException(nameof(roleModel.Name)),
      CreatedAt = roleModel.CreatedAt,
      UpdatedAt = roleModel.UpdatedAt
    };

    _dbContext.Role.Add(roleEntity);
    await _dbContext.SaveChangesAsync();

    return new RoleModel
    {
      Id = roleEntity.Id,
      Name = roleEntity.Name,
      CreatedAt = roleEntity.CreatedAt,
      UpdatedAt = roleEntity.UpdatedAt
    };
  }

  public async Task<RoleModel> UpdateAsync(int id, RoleModel roleModel)
  {
    var existingRole = await _dbContext.Role.FindAsync(id) ?? throw new ArgumentNullException(nameof(roleModel), "Role to update cannot be found");
    if (!string.IsNullOrWhiteSpace(roleModel.Name))
    {
      existingRole.Name = roleModel.Name;
    }

    existingRole.UpdatedAt = DateTime.UtcNow;

    await _dbContext.SaveChangesAsync();

    return new RoleModel
    {
      Id = existingRole.Id,
      Name = existingRole.Name,
      CreatedAt = existingRole.CreatedAt,
      UpdatedAt = existingRole.UpdatedAt
    };
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var existingRole = await _dbContext.Role.FindAsync(id);
    if (existingRole == null)
    {
      return false;
    }

    _dbContext.Role.Remove(existingRole);
    await _dbContext.SaveChangesAsync();

    return true;
  }

  private static RoleModel MapEntityToModel(RoleEntity roleEntity)
  {
    return new RoleModel
    {
      Id = roleEntity.Id,
      Name = roleEntity.Name,
      CreatedAt = roleEntity.CreatedAt,
      UpdatedAt = roleEntity.UpdatedAt
    };
  }
}