using LibraCore.Backend.Data;
using LibraCore.Backend.Entities;
using LibraCore.Backend.Models;
using LibraCore.Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraCore.Backend.Services.Implementations;

public class UserStatusService : IUserStatusService
{
  private readonly MainDBContext _dbContext;

  public UserStatusService(MainDBContext mainDBContext)
  {
    _dbContext = mainDBContext ?? throw new ArgumentNullException(nameof(mainDBContext));
  }

  public async Task<UserStatusModel?> GetAsync(int id)
  {
    var userStatusEntity = await _dbContext.UserStatus.FindAsync(id);
    return userStatusEntity != null ? UserStatusMapper.EntityToModel(userStatusEntity) : null;
  }

  public async Task<IEnumerable<UserStatusModel>> GetAllAsync()
  {
    var userStatuses = await _dbContext.UserStatus.ToListAsync();
    return userStatuses.Select(UserStatusMapper.EntityToModel);
  }

  public async Task<UserStatusModel> CreateAsync(UserStatusModel userStatusModel)
  {
    UserStatusEntity userStatusEntity = UserStatusMapper.ModelToEntity(userStatusModel);

    _dbContext.UserStatus.Add(userStatusEntity);
    await _dbContext.SaveChangesAsync();

    return UserStatusMapper.EntityToModel(userStatusEntity);
  }

  public async Task<UserStatusModel> UpdateAsync(int id, UserStatusModel userStatusModel)
  {
    var existingUserStatus = await _dbContext.UserStatus.FindAsync(id) ?? throw new ArgumentNullException(nameof(userStatusModel), "User Status to update cannot be found");
    if (!string.IsNullOrWhiteSpace(userStatusModel.Name))
    {
      existingUserStatus.Name = userStatusModel.Name;
    }

    await _dbContext.SaveChangesAsync();

    return UserStatusMapper.EntityToModel(existingUserStatus);
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var existingUserStatus = await _dbContext.UserStatus.FindAsync(id);
    if (existingUserStatus == null)
    {
      return false;
    }

    _dbContext.UserStatus.Remove(existingUserStatus);
    await _dbContext.SaveChangesAsync();

    return true;
  }
}