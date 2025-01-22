using System.ComponentModel.DataAnnotations.Schema;
using LibraCore.Backend.Entities.Base;

namespace LibraCore.Backend.Entities;

[Table("role")]
public class RoleEntity : BaseEntity
{
  public int Id { get; set; }

  public required string Name { get; set; }
}