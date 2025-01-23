using System.ComponentModel.DataAnnotations;

namespace LibraCore.Backend.DTOs.Role;

public class CreateRoleRequest
{
  [Required]
  public string? Name { get; set; }
}