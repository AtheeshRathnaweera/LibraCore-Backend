namespace LibraCore.Backend.DTOs;

public class CreateUserRequest
{
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
}