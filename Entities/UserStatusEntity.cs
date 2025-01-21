using System.ComponentModel.DataAnnotations.Schema;

namespace LibraCore.Backend.Entities;

[Table("user_status")]
public class UserStatusEntity
{
  public int Id { get; set; }

  public required string Name { get; set; }

  [Column("created_at")]
  public DateTime CreatedAt { get; set; }
  
  [Column("updated_at")]
  public DateTime? UpdatedAt { get; set; }
}