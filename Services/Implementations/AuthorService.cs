using LibraCore.Backend.Data;
using LibraCore.Backend.Entities;
using LibraCore.Backend.Mappers;
using LibraCore.Backend.Models;
using LibraCore.Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraCore.Backend.Services.Implementations;

public class AuthorService : IAuthorService
{
  private readonly MainDBContext _dbContext;

  public AuthorService(MainDBContext mainDBContext)
  {
    _dbContext = mainDBContext ?? throw new ArgumentNullException(nameof(mainDBContext));
  }

  public async Task<AuthorModel?> GetAsync(int id)
  {
    var authorEntity = await _dbContext.Author.FindAsync(id);
    return authorEntity != null ? AuthorMapper.EntityToModel(authorEntity) : null;
  }

  public async Task<IEnumerable<AuthorModel>> GetAllAsync()
  {
    var authors = await _dbContext.Author.ToListAsync();
    return authors.Select(AuthorMapper.EntityToModel);
  }

  public async Task<AuthorModel> CreateAsync(AuthorModel authorModel)
  {
    AuthorEntity authorEntity = AuthorMapper.ModelToEntity(authorModel);

    _dbContext.Author.Add(authorEntity);
    await _dbContext.SaveChangesAsync();

    return AuthorMapper.EntityToModel(authorEntity);
  }

  public async Task<AuthorModel> UpdateAsync(int id, AuthorModel authorModel)
  {
    var existingAuthor = await _dbContext.Author.FindAsync(id) ?? throw new ArgumentNullException(nameof(authorModel), "Author to update cannot be found");

    AuthorMapper.UpdateEntityFromModel(existingAuthor, authorModel);
    await _dbContext.SaveChangesAsync();

    return AuthorMapper.EntityToModel(existingAuthor);
  }

  public async Task<bool> DeleteAsync(int id)
  {
    var existingAuthor = await _dbContext.Author.FindAsync(id);
    if (existingAuthor == null)
    {
      return false;
    }

    _dbContext.Author.Remove(existingAuthor);
    await _dbContext.SaveChangesAsync();

    return true;
  }
}