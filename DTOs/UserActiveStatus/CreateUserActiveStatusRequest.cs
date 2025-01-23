using System.ComponentModel.DataAnnotations;

namespace LibraCore.Backend.DTOs.UserActiveStatus;

public class CreateUserActiveStatusRequest
{
  [Required]
  public int UserId { get; set; }

  [Required]
  public int UserStatusId { get; set; }
}