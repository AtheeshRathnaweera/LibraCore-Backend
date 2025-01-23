using System.ComponentModel.DataAnnotations;

namespace LibraCore.Backend.DTOs.Role;

public class UpdateRoleRequest
{
  [Required]
  public int Id { get; set; }

  [Required]
  public string? Name { get; set; }
}