using System.ComponentModel.DataAnnotations.Schema;
using LibraCore.Backend.Entities.Base;

namespace LibraCore.Backend.Entities;

[Table("user_status")]
public class UserStatusEntity : BaseEntity
{
  public int Id { get; set; }

  public required string Name { get; set; }
}