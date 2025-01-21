using LibraCore.Backend.Data;
using LibraCore.Backend.Entities;
using LibraCore.Backend.Models;
using LibraCore.Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraCore.Backend.Services.Implementations;

public class UserService : IUserService
{
  private readonly MainDBContext _dbContext;

  public UserService(MainDBContext mainDBContext)
  {
    _dbContext = mainDBContext ?? throw new ArgumentNullException(nameof(mainDBContext));
  }

  public async Task<UserModel?> GetAsync(int id)
  {
    var userEntity = await _dbContext.User.FindAsync(id);
    return userEntity != null ? MapEntityToModel(userEntity) : null;
  }

  public async Task<IEnumerable<UserModel>> GetAllAsync()
  {
    var users = await _dbContext.User.ToListAsync();
    return users.Select(MapEntityToModel);
  }

  public async Task<UserModel> CreateAsync(UserModel userModel)
  {
    UserEntity userEntity = new()
    {
      FirstName = userModel.FirstName,
      LastName = userModel.LastName,
      AddressOne = userModel.AddressOne,
      AddressTwo = userModel.AddressTwo,
      City = userModel.City,
      District = userModel.District,
      Email = userModel.Email,
      PhoneNumber = userModel.PhoneNumber,
      NIC = userModel.NIC,
      DoB = userModel.DoB,
      CreatedAt = userModel.CreatedAt,
      UpdatedAt = userModel.UpdatedAt
    };

    _dbContext.User.Add(userEntity);
    await _dbContext.SaveChangesAsync();

    return MapEntityToModel(userEntity);
  }

  public async Task<UserModel> UpdateAsync(int id, UserModel userModel)
  {
    var existingUser = await _dbContext.User.FindAsync(id) ?? throw new ArgumentNullException(nameof(userModel), "User to update cannot be found");

    existingUser.FirstName = userModel.FirstName;
    existingUser.LastName = userModel.LastName;
    existingUser.UpdatedAt = DateTime.UtcNow;

    await _dbContext.SaveChangesAsync();

    return MapEntityToModel(existingUser);
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var existingUser = await _dbContext.User.FindAsync(id);
    if (existingUser == null)
    {
      return false;
    }

    _dbContext.User.Remove(existingUser);
    await _dbContext.SaveChangesAsync();

    return true;
  }

  private static UserModel MapEntityToModel(UserEntity userEntity)
  {
    return new UserModel
    {
      Id = userEntity.Id,
      FirstName = userEntity.FirstName,
      LastName = userEntity.LastName,
      AddressOne = userEntity.AddressOne,
      AddressTwo = userEntity.AddressTwo,
      City = userEntity.City,
      District = userEntity.District,
      Email = userEntity.Email,
      PhoneNumber = userEntity.PhoneNumber,
      NIC = userEntity.NIC,
      DoB = userEntity.DoB,
      CreatedAt = userEntity.CreatedAt,
      UpdatedAt = userEntity.UpdatedAt
    };
  }
}