using System.ComponentModel.DataAnnotations;

namespace LibraCore.Backend.DTOs.UserStatus;

public class CreateUserStatusRequest
{
  [Required]
  public string? Name { get; set; }
}