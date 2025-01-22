using LibraCore.Backend.Data;
using LibraCore.Backend.Entities;
using LibraCore.Backend.Mappers;
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
    return userEntity != null ? UserMapper.EntityToModel(userEntity) : null;
  }

  public async Task<IEnumerable<UserModel>> GetAllAsync()
  {
    var users = await _dbContext.User.ToListAsync();
    return users.Select(UserMapper.EntityToModel);
  }

  public async Task<UserModel> CreateAsync(UserModel userModel)
  {
    UserEntity userEntity = UserMapper.ModelToEntity(userModel);

    _dbContext.User.Add(userEntity);
    await _dbContext.SaveChangesAsync();

    return UserMapper.EntityToModel(userEntity);
  }

  public async Task<UserModel> UpdateAsync(int id, UserModel userModel)
  {
    var existingUser = await _dbContext.User.FindAsync(id) ?? throw new ArgumentNullException(nameof(userModel), "User to update cannot be found");

    existingUser.FirstName = userModel.FirstName;
    existingUser.LastName = userModel.LastName;
    existingUser.AddressOne = userModel.AddressOne;
    existingUser.AddressTwo = userModel.AddressTwo;
    existingUser.City = userModel.City;
    existingUser.District = userModel.District;
    existingUser.Email = userModel.Email;
    existingUser.PhoneNumber = userModel.PhoneNumber;
    existingUser.NIC = userModel.NIC;
    existingUser.DateOfBirth = userModel.DateOfBirth;

    await _dbContext.SaveChangesAsync();

    return UserMapper.EntityToModel(existingUser);
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
}