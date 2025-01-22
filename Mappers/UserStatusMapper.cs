using LibraCore.Backend.Entities;
using LibraCore.Backend.Models;

public static class UserStatusMapper
{
  public static UserStatusModel EntityToModel(UserStatusEntity userStatusEntity)
  {
    return new UserStatusModel
    {
      Id = userStatusEntity.Id,
      Name = userStatusEntity.Name,
      CreatedAt = userStatusEntity.CreatedAt,
      UpdatedAt = userStatusEntity.UpdatedAt
    };
  }

  public static UserStatusEntity ModelToEntity(UserStatusModel userStatusModel)
  {
    return new UserStatusEntity
    {
      Id = userStatusModel.Id,
      Name = userStatusModel.Name ?? string.Empty,
      CreatedAt = userStatusModel.CreatedAt,
      UpdatedAt = userStatusModel.UpdatedAt
    };
  }
}