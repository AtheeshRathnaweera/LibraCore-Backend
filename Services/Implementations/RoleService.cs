using LibraCore.Backend.Data;
using LibraCore.Backend.Entities;
using LibraCore.Backend.Mappers;
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
    return roleEntity != null ? RoleMapper.EntityToModel(roleEntity) : null;
  }

  public async Task<IEnumerable<RoleModel>> GetAllAsync()
  {
    var roles = await _dbContext.Role.ToListAsync();
    return roles.Select(RoleMapper.EntityToModel);
  }

  public async Task<RoleModel> CreateAsync(RoleModel roleModel)
  {
    RoleEntity roleEntity = RoleMapper.ModelToEntity(roleModel);

    _dbContext.Role.Add(roleEntity);
    await _dbContext.SaveChangesAsync();

    return RoleMapper.EntityToModel(roleEntity);
  }

  public async Task<RoleModel> UpdateAsync(int id, RoleModel roleModel)
  {
    var existingRole = await _dbContext.Role.FindAsync(id) ?? throw new ArgumentNullException(nameof(roleModel), "Role to update cannot be found");

    RoleMapper.UpdateEntityFromModel(existingRole, roleModel);
    await _dbContext.SaveChangesAsync();

    return RoleMapper.EntityToModel(existingRole);
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
}