using System.ComponentModel.DataAnnotations;

namespace LibraCore.Backend.DTOs.Role;

public class UpdateAuthorRequest
{
  [Required]
  public int Id { get; set; }

  [Required]
  public string? FirstName { get; set; }

  public string? LastName { get; set; }

  public DateTime DateOfBirth { get; set; }

  public DateTime DateOfDeath { get; set; }

  public string? Nationality { get; set; }

  public string? ImageUrl { get; set; }

  public override string ToString()
  {
    return $"Id: {Id}, FirstName: {FirstName}, LastName: {LastName}, DateOfBirth: {DateOfBirth}, DateOfDeath: {DateOfDeath}, Nationality: {Nationality}, ImageUrl: {ImageUrl}";
  }
}