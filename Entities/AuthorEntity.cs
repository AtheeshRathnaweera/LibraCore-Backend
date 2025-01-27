using System.ComponentModel.DataAnnotations.Schema;
using LibraCore.Backend.Entities.Base;

namespace LibraCore.Backend.Entities;

// Note: Property validations such as required fields, non-null constraints, and other rules
// are defined in the corresponding request validator for this entity.
// You can find these validations in the following file:
// Validators/AuthorRequestValidator.cs

[Table("author")]
public class AuthorEntity : BaseEntity
{
  public int Id { get; set; }

  [Column("first_name")]
  public string? FirstName { get; set; }

  [Column("last_name")]
  public string? LastName { get; set; }

  [Column("date_of_birth")]
  public DateTime DateOfBirth { get; set; }

  [Column("date_of_death")]
  public DateTime DateOfDeath { get; set; }

  public string? Nationality { get; set; }

  [Column("image_url")]
  public string? ImageUrl { get; set; }
}