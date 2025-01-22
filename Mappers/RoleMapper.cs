using LibraCore.Backend.Entities;
using LibraCore.Backend.Models;
using LibraCore.Backend.Utilities;

namespace LibraCore.Backend.Mappers;

public static class RoleMapper
{
  public static RoleModel EntityToModel(RoleEntity roleEntity)
  {
    return new RoleModel
    {
      Id = roleEntity.Id,
      Name = roleEntity.Name,
      CreatedAt = roleEntity.CreatedAt,
      UpdatedAt = roleEntity.UpdatedAt
    };
  }

  public static RoleEntity ModelToEntity(RoleModel roleModel)
  {
    return new RoleEntity
    {
      Id = roleModel.Id,
      Name = roleModel.Name ?? string.Empty,
      CreatedAt = roleModel.CreatedAt,
      UpdatedAt = roleModel.UpdatedAt
    };
  }

  public static RoleModel DTOtoModel<T>(T DTO) where T : class
  {
    var dynamicDTO = DTO as dynamic;

    return new RoleModel
    {
      Id = CommonUtils.PropertyExists(dynamicDTO, "Id") ? dynamicDTO.Id : null,
      Name = CommonUtils.PropertyExists(dynamicDTO, "Name") ? dynamicDTO.Name : null
    };
  }

  public static void UpdateEntityFromModel(RoleEntity existingRole, RoleModel roleModel)
  {
    if (!string.IsNullOrWhiteSpace(roleModel.Name)) existingRole.Name = roleModel.Name;
  }
}