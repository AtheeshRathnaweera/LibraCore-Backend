using LibraCore.Backend.Models;

namespace LibraCore.Backend.Services.Interfaces;

public interface IUserService {
  Task<UserModel?> GetAsync(int id);

  Task<IEnumerable<UserModel>> GetAllAsync();

  Task<UserModel> CreateAsync(UserModel userModel);

  Task<UserModel> UpdateAsync(int id, UserModel userModel);
  
  Task<bool> DeleteAsync(int id);
}