using System.ComponentModel.DataAnnotations.Schema;
using LibraCore.Backend.Entities.Base;

namespace LibraCore.Backend.Entities;

// Note: Property validations such as required fields, non-null constraints, and other rules
// are defined in the corresponding request validator for this entity.
// You can find these validations in the following file:
// Validators/UserRequestValidator.cs

[Table("user")]
public class UserEntity : BaseEntity
{
  public int Id { get; set; }

  [Column("first_name")]
  public string? FirstName { get; set; }

  [Column("last_name")]
  public string? LastName { get; set; }

  [Column("address_one")]
  public string? AddressOne { get; set; }

  [Column("address_two")]
  public string? AddressTwo { get; set; }

  public string? City { get; set; }

  public string? District { get; set; }

  public string? Email { get; set; }

  [Column("phone_number")]
  public string? PhoneNumber { get; set; }

  public string? NIC { get; set; }

  [Column("date_of_birth")]
  public DateTime DateOfBirth { get; set; }
}