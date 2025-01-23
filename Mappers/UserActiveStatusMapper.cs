using LibraCore.Backend.Entities;
using LibraCore.Backend.Models;
using LibraCore.Backend.Utilities;

namespace LibraCore.Backend.Mappers;

public static class UserActiveStatusMapper
{
  public static UserActiveStatusModel EntityToModel(UserActiveStatusEntity userActiveStatusEntity)
  {
    return new UserActiveStatusModel
    {
      Id = userActiveStatusEntity.Id,
      UserId = userActiveStatusEntity.UserId,
      UserStatusId = userActiveStatusEntity.UserStatusId,
      CreatedAt = userActiveStatusEntity.CreatedAt,
      UpdatedAt = userActiveStatusEntity.UpdatedAt
    };
  }

  public static UserActiveStatusEntity ModelToEntity(UserActiveStatusModel userActiveStatusModel)
  {
    return new UserActiveStatusEntity
    {
      Id = userActiveStatusModel.Id,
      UserId = userActiveStatusModel.UserId,
      UserStatusId = userActiveStatusModel.UserStatusId,
      CreatedAt = userActiveStatusModel.CreatedAt,
      UpdatedAt = userActiveStatusModel.UpdatedAt
    };
  }

  public static UserActiveStatusModel DTOtoModel<T>(T DTO) where T : class
  {
    var dynamicDTO = DTO as dynamic;

    return new UserActiveStatusModel
    {
      Id = CommonUtils.PropertyExists(dynamicDTO, "Id") ? dynamicDTO.Id : null,
      UserId = CommonUtils.PropertyExists(dynamicDTO, "UserId") ? dynamicDTO.UserId : null,
      UserStatusId = CommonUtils.PropertyExists(dynamicDTO, "UserStatusId") ? dynamicDTO.UserStatusId : null,
      CreatedAt = CommonUtils.PropertyExists(dynamicDTO, "CreatedAt") ? dynamicDTO.CreatedAt : null,
      UpdatedAt = CommonUtils.PropertyExists(dynamicDTO, "UpdatedAt") ? dynamicDTO.UpdatedAt : null
    };
  }

  public static void UpdateEntityFromModel(UserActiveStatusEntity existingUserActiveStatusEntity, UserActiveStatusModel userActiveStatusModel)
  {
    if (userActiveStatusModel.UserId != default(int)) existingUserActiveStatusEntity.UserId = userActiveStatusModel.UserId;
    if (userActiveStatusModel.UserStatusId != default(int)) existingUserActiveStatusEntity.UserStatusId = userActiveStatusModel.UserStatusId;
  }
}