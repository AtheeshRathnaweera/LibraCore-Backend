using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibraCore.Backend.Entities.Base;

namespace LibraCore.Backend.Entities;

// Note: Property validations such as required fields, non-null constraints, and other rules
// are defined in the corresponding request validator for this entity.
// You can find these validations in the following file:
// Validators/UserActiveStatusRequestValidators.cs

[Table("user_active_status")]
public class UserActiveStatusEntity : BaseEntity
{
  public int Id { get; set; }

  [Column("user_id")]
  public required int UserId { get; set; }

  [ForeignKey("UserId")]
  [JsonIgnore] // Ignore the foreign key reference during serialization to avoid circular references
  public virtual UserEntity? User { get; set; }

  [Column("user_status_id")]
  public required int UserStatusId { get; set; }

  [ForeignKey("UserStatusId")]
  [JsonIgnore] // Ignore the foreign key reference during serialization
  public virtual UserStatusEntity? UserStatus { get; set; }
}