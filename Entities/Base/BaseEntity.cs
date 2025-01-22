using System.ComponentModel.DataAnnotations.Schema;

namespace LibraCore.Backend.Entities.Base;

public abstract class BaseEntity
{
  [Column("created_at")]
  public DateTime CreatedAt { get; set; }

  [Column("updated_at")]
  public DateTime? UpdatedAt { get; set; }
}