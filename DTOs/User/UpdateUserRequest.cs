namespace LibraCore.Backend.DTOs.User;

public class UpdateUserRequest
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

  public DateTime DateOfBirth { get; set; }

  public override string ToString()
  {
    return $"Id: {Id}, FirstName: {FirstName}, LastName: {LastName}, AddressOne: {AddressOne}, AddressTwo: {AddressTwo}, City: {City}, District: {District}, Email: {Email}, PhoneNumber: {PhoneNumber}, NIC: {NIC}, DateOfBirth: {DateOfBirth}";
  }
}