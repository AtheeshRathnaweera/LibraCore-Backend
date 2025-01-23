using LibraCore.Backend.Models;

namespace LibraCore.Backend.Services.Interfaces;

public interface IUserActiveStatusService
{
  Task<UserActiveStatusModel?> GetAsync(int id, string? expand);

  Task<IEnumerable<UserActiveStatusModel>> GetAllAsync(string? expand);

  Task<UserActiveStatusModel> CreateAsync(UserActiveStatusModel userActiveStatusModel);

  Task<UserActiveStatusModel> UpdateAsync(int id, UserActiveStatusModel userActiveStatusModel);

  Task<bool> DeleteAsync(int id);
}