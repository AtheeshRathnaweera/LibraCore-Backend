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
    existingAuthor.LastName = authorModel.LastName;
    existingAuthor.DateOfBirth = authorModel.DateOfBirth;
    existingAuthor.DateOfDeath = authorModel.DateOfDeath;
    existingAuthor.Nationality = authorModel.Nationality;
    existingAuthor.ImageUrl = authorModel.ImageUrl;
  }
}