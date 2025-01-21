using System.ComponentModel.DataAnnotations.Schema;

namespace LibraCore.Backend.Entities;

public class UserEntity{
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

  public DateTime DoB { get; set; }

  [Column("created_at")]
  public DateTime CreatedAt { get; set; }

  [Column("updated_at")]
  public DateTime? UpdatedAt { get; set; }
}