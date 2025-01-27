using LibraCore.Backend.Models;

namespace LibraCore.Backend.Services.Interfaces;

public interface IAuthorService
{
  Task<AuthorModel?> GetAsync(int id);

  Task<IEnumerable<AuthorModel>> GetAllAsync();

  Task<AuthorModel> CreateAsync(AuthorModel authorModel);

  Task<AuthorModel> UpdateAsync(int id, AuthorModel authorModel);

  Task<bool> DeleteAsync(int id);
}