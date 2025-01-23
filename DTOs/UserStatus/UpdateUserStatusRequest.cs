using System.ComponentModel.DataAnnotations;

namespace LibraCore.Backend.DTOs.UserStatus;

public class UpdateUserStatusRequest
{
  [Required]
  public int Id { get; set; }

  [Required]
  public string? Name { get; set; }
}