using System.ComponentModel.DataAnnotations.Schema;
using LibraCore.Backend.Entities.Base;

namespace LibraCore.Backend.Entities;

// Note: Property validations such as required fields, non-null constraints, and other rules
// are defined in the corresponding request validator for this entity.
// You can find these validations in the following file:
// Validators/RoleRequestValidators.cs

[Table("role")]
public class RoleEntity : BaseEntity
{
  public int Id { get; set; }

  public required string Name { get; set; }
}