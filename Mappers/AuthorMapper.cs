using LibraCore.Backend.Entities;
using LibraCore.Backend.Models;

namespace LibraCore.Backend.Mappers;

public static class AuthorMapper
{
  public static AuthorModel EntityToModel(AuthorEntity authorEntity)
  {
    return new AuthorModel
    {
      Id = authorEntity.Id,
      FirstName = authorEntity.FirstName,
      LastName = authorEntity.LastName,
      DateOfBirth = authorEntity.DateOfBirth,
      DateOfDeath = authorEntity.DateOfDeath,
      Nationality = authorEntity.Nationality,
      ImageUrl = authorEntity.ImageUrl,
      CreatedAt = authorEntity.CreatedAt,
      UpdatedAt = authorEntity.UpdatedAt
    };
  }

  public static AuthorEntity ModelToEntity(AuthorModel authorModel)
  {
    return new AuthorEntity
    {
      Id = authorModel.Id,
      FirstName = authorModel.FirstName,
      LastName = authorModel.LastName,
      DateOfBirth = authorModel.DateOfBirth,
      DateOfDeath = authorModel.DateOfDeath,
      Nationality = authorModel.Nationality,
      ImageUrl = authorModel.ImageUrl,
      CreatedAt = authorModel.CreatedAt,
      UpdatedAt = authorModel.UpdatedAt
    };
  }

  public static void UpdateEntityFromModel(AuthorEntity existingAuthor, AuthorModel authorModel)
  {
    if (!string.IsNullOrWhiteSpace(authorModel.FirstName)) existingAuthor.FirstName = authorModel.FirstName;
    if (!string.IsNullOrWhiteSpace(authorModel.LastName)) existingAuthor.LastName = authorModel.LastName;
    if (authorModel.DateOfBirth != default) existingAuthor.DateOfBirth = authorModel.DateOfBirth;
    if (authorModel.DateOfDeath != default) existingAuthor.DateOfDeath = authorModel.DateOfDeath;
    if (!string.IsNullOrWhiteSpace(authorModel.Nationality)) existingAuthor.Nationality = authorModel.Nationality;
    if (!string.IsNullOrWhiteSpace(authorModel.ImageUrl)) existingAuthor.ImageUrl = authorModel.ImageUrl;
  }
}