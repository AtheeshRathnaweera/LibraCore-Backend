using LibraCore.Backend.Models;

namespace LibraCore.Backend.Services.Interfaces;

public interface IUserStatusService
{
  Task<UserStatusModel?> GetAsync(int id);
  Task<IEnumerable<UserStatusModel>> GetAllAsync();
  Task<UserStatusModel> CreateAsync(UserStatusModel userStatusModel);
  Task<UserStatusModel> UpdateAsync(int id, UserStatusModel userStatusModel);
  Task<bool> DeleteAsync(int id);
}