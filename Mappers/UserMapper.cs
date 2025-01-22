using LibraCore.Backend.Entities;
using LibraCore.Backend.Models;
using LibraCore.Backend.Utilities;

namespace LibraCore.Backend.Mappers;

public static class UserMapper
{
  public static UserModel EntityToModel(UserEntity userEntity)
  {
    return new UserModel
    {
      Id = userEntity.Id,
      FirstName = userEntity.FirstName,
      LastName = userEntity.LastName,
      AddressOne = userEntity.AddressOne,
      AddressTwo = userEntity.AddressTwo,
      City = userEntity.City,
      District = userEntity.District,
      Email = userEntity.Email,
      PhoneNumber = userEntity.PhoneNumber,
      NIC = userEntity.NIC,
      DateOfBirth = userEntity.DateOfBirth,
      CreatedAt = userEntity.CreatedAt,
      UpdatedAt = userEntity.UpdatedAt
    };
  }

  public static UserEntity ModelToEntity(UserModel userModel)
  {
    return new UserEntity
    {
      Id = userModel.Id,
      FirstName = userModel.FirstName,
      LastName = userModel.LastName,
      AddressOne = userModel.AddressOne,
      AddressTwo = userModel.AddressTwo,
      City = userModel.City,
      District = userModel.District,
      Email = userModel.Email,
      PhoneNumber = userModel.PhoneNumber,
      NIC = userModel.NIC,
      DateOfBirth = userModel.DateOfBirth,
      CreatedAt = userModel.CreatedAt,
      UpdatedAt = userModel.UpdatedAt
    };
  }

  public static UserModel DTOtoModel<T>(T DTO) where T : class
  {
    var dynamicDTO = DTO as dynamic;

    return new UserModel
    {
      Id = CommonUtils.PropertyExists(dynamicDTO, "Id") ? dynamicDTO.Id : 0,
      FirstName = CommonUtils.PropertyExists(dynamicDTO, "FirstName") ? dynamicDTO.FirstName : null,
      LastName = CommonUtils.PropertyExists(dynamicDTO, "LastName") ? dynamicDTO.LastName : null,
      AddressOne = CommonUtils.PropertyExists(dynamicDTO, "AddressOne") ? dynamicDTO.AddressOne : null,
      AddressTwo = CommonUtils.PropertyExists(dynamicDTO, "AddressTwo") ? dynamicDTO.AddressTwo : null,
      City = CommonUtils.PropertyExists(dynamicDTO, "City") ? dynamicDTO.City : null,
      District = CommonUtils.PropertyExists(dynamicDTO, "District") ? dynamicDTO.District : null,
      Email = CommonUtils.PropertyExists(dynamicDTO, "Email") ? dynamicDTO.Email : null,
      PhoneNumber = CommonUtils.PropertyExists(dynamicDTO, "PhoneNumber") ? dynamicDTO.PhoneNumber : null,
      NIC = CommonUtils.PropertyExists(dynamicDTO, "NIC") ? dynamicDTO.NIC : null,
      DateOfBirth = CommonUtils.PropertyExists(dynamicDTO, "DateOfBirth") ? dynamicDTO.DateOfBirth : DateTime.MinValue,
      CreatedAt = CommonUtils.PropertyExists(dynamicDTO, "CreatedAt") ? dynamicDTO.CreatedAt : DateTime.MinValue,
      UpdatedAt = CommonUtils.PropertyExists(dynamicDTO, "UpdatedAt") ? dynamicDTO.UpdatedAt : null
    };
  }
}