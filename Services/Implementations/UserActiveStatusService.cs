using LibraCore.Backend.Data;
using LibraCore.Backend.Entities;
using LibraCore.Backend.Mappers;
using LibraCore.Backend.Models;
using LibraCore.Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraCore.Backend.Services.Implementations;

public class UserActiveStatusService : IUserActiveStatusService
{
  private readonly MainDBContext _dbContext;

  public UserActiveStatusService(MainDBContext mainDBContext)
  {
    _dbContext = mainDBContext ?? throw new ArgumentNullException(nameof(mainDBContext));
  }

  public async Task<UserActiveStatusModel?> GetAsync(int id, string? expand)
  {
    var query = _dbContext.UserActiveStatus.AsQueryable();

    query = ApplyExpandQueryParams(query, expand);

    var userActiveStatus = await query.FirstOrDefaultAsync(uas => uas.Id == id);
    return userActiveStatus != null ? UserActiveStatusMapper.EntityToModel(userActiveStatus) : null;
  }

  public async Task<IEnumerable<UserActiveStatusModel>> GetAllAsync(string? expand)
  {
    var query = _dbContext.UserActiveStatus.AsQueryable();

    query = ApplyExpandQueryParams(query, expand);

    var userActiveStatuses = await query.ToListAsync();
    return userActiveStatuses.Select(UserActiveStatusMapper.EntityToModel);
  }

  public async Task<UserActiveStatusModel> CreateAsync(UserActiveStatusModel userActiveStatusModel)
  {
    UserActiveStatusEntity userActiveStatusEntity = UserActiveStatusMapper.ModelToEntity(userActiveStatusModel);

    _dbContext.UserActiveStatus.Add(userActiveStatusEntity);
    await _dbContext.SaveChangesAsync();

    return UserActiveStatusMapper.EntityToModel(userActiveStatusEntity);
  }

  public async Task<UserActiveStatusModel> UpdateAsync(int id, UserActiveStatusModel userActiveStatusModel)
  {
    var existingUserActiveStatus = await _dbContext.UserActiveStatus.FindAsync(id) ?? throw new ArgumentNullException(nameof(userActiveStatusModel), "User Active Status to update cannot be found");

    UserActiveStatusMapper.UpdateEntityFromModel(existingUserActiveStatus, userActiveStatusModel);
    await _dbContext.SaveChangesAsync();

    return UserActiveStatusMapper.EntityToModel(existingUserActiveStatus);
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var existingUserActiveStatus = await _dbContext.UserActiveStatus.FindAsync(id);
    if (existingUserActiveStatus == null)
    {
      return false;
    }

    _dbContext.UserActiveStatus.Remove(existingUserActiveStatus);
    await _dbContext.SaveChangesAsync();

    return true;
  }

  private static IQueryable<UserActiveStatusEntity> ApplyExpandQueryParams(IQueryable<UserActiveStatusEntity> query, string? expand)
  {
    if (!string.IsNullOrEmpty(expand))
    {
      var expandProperties = expand.Split(',');

      if (expandProperties.Contains("user")) query = query.Include(uas => uas.User);
      if (expandProperties.Contains("userStatus")) query = query.Include(uas => uas.UserStatus);
    }
    return query;
  }
}