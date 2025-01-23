using System.ComponentModel.DataAnnotations;

namespace LibraCore.Backend.DTOs.User;

public class UpdateUserRequest
{
  [Required]
  public int Id { get; set; }

  [Required]
  public string? FirstName { get; set; }
  
  public string? LastName { get; set; }

  [Required]
  public string? AddressOne { get; set; }

  public string? AddressTwo { get; set; }

  [Required]
  public string? City { get; set; }

  public string? District { get; set; }

  [Required]
  public string? Email { get; set; }

  [Required]
  public string? PhoneNumber { get; set; }

  public string? NIC { get; set; }

  [Required]
  public DateTime DateOfBirth { get; set; }

  public override string ToString()
  {
    return $"Id: {Id}, FirstName: {FirstName}, LastName: {LastName}, AddressOne: {AddressOne}, AddressTwo: {AddressTwo}, City: {City}, District: {District}, Email: {Email}, PhoneNumber: {PhoneNumber}, NIC: {NIC}, DateOfBirth: {DateOfBirth}";
  }
}