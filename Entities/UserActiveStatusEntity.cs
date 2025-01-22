using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using LibraCore.Backend.Entities.Base;

namespace LibraCore.Backend.Entities;

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