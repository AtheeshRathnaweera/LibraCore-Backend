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

  public DateTime DateOfBirth { get; set; }

  public DateTime CreatedAt { get; set; }

  public DateTime? UpdatedAt { get; set; }

  public UserModel()
  {
  }

  public UserModel(
    string? firstName, string? lastName, string? addressOne, string? addressTwo,
    string? city, string? district, string? email, string? phoneNumber, string? nic,
    DateTime dateOfBirth)
  {
    FirstName = firstName;
    LastName = lastName;
    AddressOne = addressOne;
    AddressTwo = addressTwo;
    City = city;
    District = district;
    Email = email;
    PhoneNumber = phoneNumber;
    NIC = nic;
    DateOfBirth = dateOfBirth;
  }

  public override string ToString()
  {
    return $"UserModel: Id={Id}, FirstName={FirstName}, LastName={LastName}, AddressOne={AddressOne}, AddressTwo={AddressTwo}, City={City}, District={District}, Email={Email}, PhoneNumber={PhoneNumber}, NIC={NIC}, DateOfBirth={DateOfBirth}, CreatedAt={CreatedAt}, UpdatedAt={UpdatedAt}";
  }
}