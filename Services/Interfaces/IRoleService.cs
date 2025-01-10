using LibraCore.Backend.Entities;
using LibraCore.Backend.Models;

namespace LibraCore.Backend.Services;

public interface IRoleService
{
    Task<RoleModel?> GetAsync(int id);
    Task<IEnumerable<RoleModel>> GetAllAsync();
    Task<RoleModel> CreateAsync(RoleModel roleModel);
    Task<RoleModel> UpdateAsync(int id, RoleModel roleModel);
    Task<bool> DeleteAsync(int id);
}