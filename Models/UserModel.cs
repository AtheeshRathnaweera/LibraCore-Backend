namespace LibraCore.Backend.Models;

public class UserModel
{
  public int Id { get; set; }

  public string? FirstName { get; set; }

  public string? LastName { get; set; }

  public string? AddressOne { get; set; }

  public string? AddressTwo { get; set; }

  public string? City { get; set; }

  public string? District { get; set; }

  public string? Email { get; set; }

  public string? PhoneNumber { get; set; }

  public string? NIC { get; set; }

  public DateTime DoB { get; set; }

  public DateTime CreatedAt { get; set; }

  public DateTime? UpdatedAt { get; set; }

  public UserModel()
  {

  }

  public UserModel(string? firstName, string? lastName)
  {
    FirstName = firstName;
    LastName = lastName;
  }

  public UserModel(string? firstName, string? lastName, string? email, string? phoneNumber, string? nic, DateTime doB, DateTime createdAt, DateTime? updatedAt)
  {
    FirstName = firstName;
    LastName = lastName;

    Email = email;
    PhoneNumber = phoneNumber;
    NIC = nic;
    DoB = doB;
    CreatedAt = createdAt;
    UpdatedAt = updatedAt;
  }
}