namespace LibraCore.Backend.Models;

public class AuthorModel
{
  public int Id { get; set; }

  public string? FirstName { get; set; }

  public string? LastName { get; set; }

  public DateTime DateOfBirth { get; set; }

  public DateTime DateOfDeath { get; set; }

  public string? Nationality { get; set; }

  public string? ImageUrl { get; set; }

  public DateTime CreatedAt { get; set; }

  public DateTime? UpdatedAt { get; set; }

  public override string ToString()
  {
    return $"AuthorModel {{ Id = {Id}, FirstName = {FirstName}, LastName = {LastName}, DateOfBirth = {DateOfBirth}, DateOfDeath = {DateOfDeath}, Nationality = {Nationality}, ImageUrl = {ImageUrl}, CreatedAt = {CreatedAt}, UpdatedAt = {UpdatedAt} }}";
  }
}