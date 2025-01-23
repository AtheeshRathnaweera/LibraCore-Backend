using System.ComponentModel.DataAnnotations;

namespace LibraCore.Backend.DTOs.UserActiveStatus;

public class UpdateUserActiveStatusRequest
{
  [Required]
  public int Id { get; set; }

  [Required]
  public int UserId { get; set; }

  [Required]
  public int UserStatusId { get; set; }
}